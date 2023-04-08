using LMS_library.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using System.Security.Claims;

namespace LMS_library.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize (Roles = "Teacher")]
    public class ResourceController : ControllerBase
    {
        private readonly IResourceRepository _repository;
        private readonly DataDBContex _contex;
        private readonly INotificationRepository _notificationRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ResourceController(INotificationRepository notificationRepository, IHttpContextAccessor httpContextAccessor, IResourceRepository repository, DataDBContex contex)
        {
            _repository = repository;
            _contex = contex;
            _notificationRepository = notificationRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetAll(int id)
        {
            try
            {
                return Ok(await _repository.GetAll(id));
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAllOnLesson(int id)
        {

            try
            {
                var resource = await _repository.GetAllOnLesson(id);
                return resource == null ? NotFound() : Ok(resource);
            }
            catch { return BadRequest(); }

        }

        [HttpPost("add-resource")]
        public async Task<IActionResult> AddNewRole(ResourceModel model)
        {
            try
            {
                await _notificationRepository.AddNotification($"Resource added successfully for {model.lessonName} at {DateTime.Now.ToLocalTime()}", Int32.Parse(UserInfo()), false);
                var newResource = await _repository.AddRessourceAsync(model);
                return Ok(newResource);
            }
            catch { return BadRequest(); }
        }
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {

            try
            {
                var resource = await _contex.ResourceLists!.FindAsync(id);
                await _notificationRepository.AddNotification($"Resource of {resource.Lesson.name} deleted at {DateTime.Now.ToLocalTime()}", Int32.Parse(UserInfo()), false);
                await _repository.Delete(id);
                return Ok("Delete Success !");

            }
            catch { return BadRequest(); }

        }
        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ResourceModel model)
        {
            try
            {
                var resource = await _contex.ResourceLists!.FindAsync(id);
                if (model.id != id)
                {
                    return NotFound();
                }
                await _notificationRepository.AddNotification($"Edit resource successfully at {DateTime.Now.ToLocalTime()}", Int32.Parse(UserInfo()), false);
                await _repository.Update(id, model);
                return Ok("Update Successfully");
            }
            catch
            {
                return BadRequest();
            }
        }


        [HttpPut("add-to-file-to-resource/{id}")]
        public async Task<IActionResult> AddToTopic(string lessonName, int id)
        {
            try
            {
                var file = await _contex.Materials!.FirstOrDefaultAsync(u => u.id == id);
                var fileType = await _contex.MaterialTypes.FirstOrDefaultAsync(u => u.name == "Resource");
                if (file.materialTypeID != fileType.id)
                {
                    return BadRequest(" File type must be resource not lesson ! ");
                }
                if (file == null)
                {
                    return NotFound();
                }
                if (lessonName == null)
                {
                    return BadRequest("Please Enter Lesson Name");
                }
                if (file.fileStatus == CourseMaterial.FileStatus.Pendding || file.fileStatus == CourseMaterial.FileStatus.Reject)
                {
                    return BadRequest("File status must be approved");
                }
                await _notificationRepository.AddNotification($"Add file to resource successfully at {DateTime.Now.ToLocalTime()},file name is {file.name}", Int32.Parse(UserInfo()), false);
                await _repository.AddToResource(lessonName, id);

                return Ok("Update Successfully");
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("download-file/{id}")]
        public async Task<IActionResult> Download(int id)// download file by resource id
        {
            try
            {
                var file = await _contex.Materials.FirstOrDefaultAsync(u => u.resourceId == id);
                if (file == null)
                {
                    return NotFound();
                }
                // create a memorystream
                var memoryStream = new MemoryStream();

                using (var stream = new FileStream(file.materialPath, FileMode.Open))
                {
                    await stream.CopyToAsync(memoryStream);
                }
                // set the position to return the file from
                memoryStream.Position = 0;
                await _notificationRepository.AddNotification($"Download resource file at {DateTime.Now.ToLocalTime()},file name is {file.name}", Int32.Parse(UserInfo()), false);
                return File(memoryStream, MimeTypes.GetMimeType(file.materialPath), file.materialPath);
            }
            catch
            {
                return BadRequest();
            }

        }

        private string UserInfo()
        {
            var result = _httpContextAccessor!.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier);
            return result;
        }

    }
}
