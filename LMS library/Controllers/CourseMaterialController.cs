using LMS_library.Data;
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
    [Authorize]

    public class CourseMaterialController : ControllerBase
    {

        private readonly ICourseMaterialRepository _repository;
        private readonly INotificationRepository _notificationRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly DataDBContex _contex;

        public CourseMaterialController(INotificationRepository notificationRepository, IHttpContextAccessor httpContextAccessor, ICourseMaterialRepository repository, DataDBContex contex)
        {
            _repository = repository;
            _contex = contex;
            _notificationRepository = notificationRepository;
            _httpContextAccessor = httpContextAccessor;
        }


        [Authorize(Roles = "Leader,Teacher")]
        [HttpPost("upload-file")]
        public async Task<IActionResult> PostMultiFile( string type , string course , List<IFormFile> FileUpload)//upload lesson or resourse , type = file type (lesson or resourse) , course = course name
        {
            try
            {
                var leader = await _contex.Users.Where(l => l.Role.name=="Leader").ToListAsync();
                foreach( var l in leader)
                {
                    await _notificationRepository.AddNotification($"New file for {course} upload at {DateTime.Now.ToLocalTime()} please approve/reject the file soon as you can !", l.id, false);
                }
                await _notificationRepository.AddNotification($"Upload file for {course} successfully at {DateTime.Now.ToLocalTime()}", Int32.Parse(UserInfo()), false);
                await _repository.PostMultiFileAsync( type , course, FileUpload);

                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        [Authorize(Roles = "Leader,Student,Teacher")]
        [HttpGet("approved-list-student")]
        public async Task<IActionResult> GetAllFileForStudent(int id)//course id  , student cant see the pendding or reject file ,ONLY SEE THE FILE BEEN APPROVED
        {
            try
            {
                return Ok(await _repository.GetAllForStudent(id));
            }
            catch
            {
                return BadRequest();
            }
        }
        [Authorize(Roles = "Leader")]
        [HttpGet("list-course-material")]
        public async Task<IActionResult> GetAllFile()//course id  , 
        {
            try
            {
                return Ok(await _repository.GetAll();
            }
            catch
            {
                return BadRequest();
            }
        }
        [Authorize(Roles = "Leader,Teacher")]
        [HttpGet("list-course-material")]
        public async Task<IActionResult> GetAllCourseFile(int id)//course id  , 
        {
            try
            {
                return Ok(await _repository.GetAllBaseOnCourse(id));
            }
            catch
            {
                return BadRequest();
            }
        }
        [Authorize(Roles = "Leader,Teacher")]
        [HttpGet("list-resource")]
        public async Task<IActionResult> GetAllResource()//course id  , 
        {
            try
            {
                return Ok(await _repository.GetAllResource());
            }
            catch
            {
                return BadRequest();
            }
        }
        [Authorize(Roles = "Leader,Teacher")]
        [HttpGet("list-course-material-teacher")]
        public async Task<IActionResult> GetallForTeahcher()//course id  , 
        {
            try
            {
                return Ok(await _repository.GetAllForTeacher());
            }
            catch
            {
                return BadRequest();
            }
        }
        [Authorize(Roles = "Leader,Teacher")]
        [HttpGet("list-lesson")]
        public async Task<IActionResult> GetAllLesson()//course id  , 
        {
            try
            {
                return Ok(await _repository.GetAllLesson());
            }
            catch
            {
                return BadRequest();
            }
        }

        [Authorize(Roles = "Leader")]
        [HttpPut("approve/{id}")]
        public async Task<IActionResult> FileApprove(string check,int id) // check = file status (approved or reject),id = file id (lesson or resourse) ,leader can approved or rejected the file 
        {
            try
            {
                var file = await _contex.Materials!.FirstOrDefaultAsync(u => u.id == id);
                if (file == null)
                {
                    return NotFound();
                }
                if (check == null)
                {
                    return BadRequest("Please Enter Choose Approve Or Reject File");
                }
                if(check == "Approved")
                {
                    await _notificationRepository.AddNotification($"File {file.name} from {file.courses.courseName} approved at {DateTime.Now.ToLocalTime()}", Int32.Parse(UserInfo()), false);
                    await _notificationRepository.AddNotification($"Your file {file.name} has been approved at {DateTime.Now.ToLocalTime()}", file.User.id, false);
                }
                if(check == "Reject")
                {
                    await _notificationRepository.AddNotification($"File {file.name} from {file.courses.courseName} reject at {DateTime.Now.ToLocalTime()}", Int32.Parse(UserInfo()), false);
                    await _notificationRepository.AddNotification($"Your file {file.name} has been reject by leader at {DateTime.Now.ToLocalTime()}", file.User.id, false);
                }
                await _repository.FileApprove(check, id);

                return Ok("Update Successfully");
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpDelete("delete/{id}")]
        [Authorize(Roles = "Leader,Teacher")]
        public async Task<IActionResult> DeleteFile([FromRoute] int id) // file id 
        {
            try
            {
                var file = await _contex.Materials.FirstOrDefaultAsync(u => u.id == id);
                if (file == null) { return NotFound(); }

                System.IO.File.Delete(file.materialPath);
                await _notificationRepository.AddNotification($"File {file.name} deleted at {DateTime.Now.ToLocalTime()}", Int32.Parse(UserInfo()), false);
                await _repository.DeleteFileAsync(id);
                return Ok();
            }
            catch { return BadRequest(); }

        }


        [Authorize(Roles = "Leader,Teacher")]
        [HttpPut("update-file-name/{id}")]
        public async Task<IActionResult> UpdateName(string newName, int id)// change file name , newName = new file name , id = file id
        {
            try
            {
                var file = await _contex.Materials!.FirstOrDefaultAsync(u => u.id == id);
                if (file == null)
                {
                    return NotFound();
                }
                if (newName == null)
                {
                    return BadRequest("Please Enter File Name");
                }
                await _notificationRepository.AddNotification($"File {file.name} has been change to {newName} at {DateTime.Now.ToLocalTime()}", Int32.Parse(UserInfo()), false);
                await _repository.UpdateFileAsync(newName, id);

                return Ok("Update Successfully");
            }
            catch
            {
                return BadRequest();
            }
        }

        [Authorize(Roles = "Leader,Teacher")]
        [HttpPut("add-to-topic/{id}")]
        public async Task<IActionResult> AddToTopic(string topic, int id)// change file name , newName = new file name , id = file id
        {
            try
            {
                var file = await _contex.Materials!.FirstOrDefaultAsync(u => u.id == id);
                if (file == null)
                {
                    return NotFound();
                }
                if (topic == null)
                {
                    return BadRequest("Please Enter File Name");
                }
                await _notificationRepository.AddNotification($"File {file.name} has been add to {topic} at {DateTime.Now.ToLocalTime()}", Int32.Parse(UserInfo()), false);
                await _repository.AddToResource(topic, id);

                return Ok("Update Successfully");
            }
            catch
            {
                return BadRequest();
            }
        }


        [HttpGet("download-file/{id}")]
        [Authorize(Roles = "Leader,Teacher,Student")]
        public async Task<IActionResult> Download(int id)// download file by file id
        {
            try
            {
                var file = await _contex.Materials.FirstOrDefaultAsync(u => u.id == id);
                if (file == null)
                {
                    return NotFound();
                }
                if(file.fileStatus != CourseMaterial.FileStatus.Approved)
                {
                    return BadRequest();
                }
                // create a memorystream
                var memoryStream = new MemoryStream();

                using (var stream = new FileStream(file.materialPath, FileMode.Open))
                {
                    await stream.CopyToAsync(memoryStream);
                }
                // set the position to return the file from
                memoryStream.Position = 0;
                await _notificationRepository.AddNotification($"File {file.name} dowloaded at {DateTime.Now.ToLocalTime()}", Int32.Parse(UserInfo()), false);
                return File(memoryStream, MimeTypes.GetMimeType(file.materialPath), file.materialPath);
            }
            catch
            {
                return BadRequest();
            }



        }

        [Authorize(Roles = "Leader,Teacher,Student")]
        [HttpGet("search-sort")]
        public async Task<IActionResult> SearchSort(string? course, string? teacher, string? status)
        {
            try
            {
                return Ok(await _repository.SearchSort(course, teacher, status));
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
