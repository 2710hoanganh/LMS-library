using LMS_library.Data;
using LMS_library.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LMS_library.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles ="Leader")]
    public class ClassController : ControllerBase
    {
        private readonly DataDBContex _contex;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IClassRepository _repository;
        private readonly INotificationRepository _notificationRepository;
        public ClassController(INotificationRepository notificationRepository, IClassRepository repository, DataDBContex contex, IHttpContextAccessor httpContextAccessor)
        {

            _contex = contex;
            _httpContextAccessor = httpContextAccessor;
            _repository= repository;
            _notificationRepository= notificationRepository;
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetAllClass()
        {
            try
            {
                return Ok(await _repository.GetAll());
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("list-base-on-teacher")]
        public async Task<IActionResult> GetAllClassBaseOnTeacher(string teaccher)//teacher email
        {
            try
            {
                return Ok(await _repository.GetAllClassBaseOnTeacher(teaccher));
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet("list-base-on-course")]
        public async Task<IActionResult> GetAllClassBaseOnCourse(int id)//course id
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
        [HttpGet("{id}")]
        public async Task<IActionResult> GetClassById(int id)
        {

            try
            {
                var Class = await _repository.GetById(id);
                return Class == null ? NotFound() : Ok(Class);
            }
            catch { return BadRequest(); }

        }
        [HttpPost("add-class")]
        public async Task<IActionResult> AddNewClass(ClassModel model)
        {
            try
            {
                if (_contex.Classes.Any(r => r.className == model.className))
                {
                    return BadRequest("Class already exists .");
                }
                await _notificationRepository.AddNotification($"Class {model.className} create successfully at {DateTime.Now.ToLocalTime()}", Int32.Parse(UserInfo()), false);
                var newRole = await _repository.AddClassAsync(model);
                return Ok(newRole);
            }
            catch { return BadRequest(); }
        }
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteClass([FromRoute] int id)
        {

            try
            {

                var classInfo = await _contex.Classes!.FindAsync(id);
                var user = await _contex.Users.ToListAsync();
                if (classInfo == null) { return NotFound(); }
                foreach(var u in user)
                {
                    await _notificationRepository.AddNotification($"Your class {classInfo.className} deleted at {DateTime.Now.ToLocalTime()}", u.id, false);

                }
                await _notificationRepository.AddNotification($"Class {classInfo.className} deleted at {DateTime.Now.ToLocalTime()}", Int32.Parse(UserInfo()), false);

                await _repository.DeleteClassAsync(id);
                return Ok("Delete Success !");

            }
            catch { return BadRequest(); }

        }
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateClass(int id, [FromBody] ClassModel model)
        {
            try
            {
                var classInfo = await _contex.Classes!.FirstOrDefaultAsync(r => r.id == id);
                if (model.id != id || classInfo == null)
                {
                    return NotFound();
                }
                await _notificationRepository.AddNotification($"Update class {classInfo.classCode} successfully at {DateTime.Now.ToLocalTime()}", Int32.Parse(UserInfo()), false);
                await _repository.UpdateClassAsync(id, model);
                return Ok("Update Successfully");
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
