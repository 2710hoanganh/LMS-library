using LMS_library.Data;
using LMS_library.Repositories;
using MailKit.Search;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LMS_library.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,Leader, Teacher")]
    public class CourseController : ControllerBase
    {
        private readonly ICourseRepository _repository;
        private readonly INotificationRepository _notificationRepository;
        private readonly DataDBContex _contex;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CourseController(IHttpContextAccessor httpContextAccessor, INotificationRepository notificationRepository, ICourseRepository repository, DataDBContex contex)
        {
            _repository = repository;
            _contex = contex;
            _notificationRepository = notificationRepository;
            _httpContextAccessor = httpContextAccessor;
        }
        [Authorize(Roles = "Admin,Leader")]
        [HttpPost("add-course")]
        public async Task<IActionResult> AddNewCourse(CourseModel model)
        {
            try
            {
                if (_contex.Courses.Any(r => r.courseName == model.courseName))
                {
                    return BadRequest("Course already exists .");
                }
                await _notificationRepository.AddNotification($"{model.courseName} create successfully at {DateTime.Now.ToLocalTime}", Int32.Parse(UserInfo()), false);
                var newCourse = await _repository.AddCourseAsync(model);
                return Ok(newCourse);
            }
            catch { return BadRequest(); }
        }
        [Authorize(Roles = "Admin,Leader")]

        [HttpGet("list")]
        public async Task<IActionResult> GetAllCourse()
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
        [HttpGet("teacher-course-list")]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> GetAllCourseForTeacher()// get the list of subjects taught by the teacher by teacher id
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
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCourse(int id)
        {

            try
            {
                var course = await _repository.GetById(id);
                return course == null ? NotFound() : Ok(course);
            }
            catch { return BadRequest(); }

        }
        [Authorize(Roles = "Admin,Leader")]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteCourse([FromRoute] int id)
        {

            try
            {
                var course = await _contex.Courses!.FindAsync(id);
                if (course == null) { return BadRequest(); }
                await _notificationRepository.AddNotification($"{course.courseName} deleted at {DateTime.Now.ToLocalTime}", Int32.Parse(UserInfo()), false);
                await _repository.DeleteCourseAsync(id);
                return Ok("Delete Success !");

            }
            catch { return BadRequest(); }

        }
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateCourse(int id, [FromBody] CourseModel model)
        {
            try
            {
                var course = await _contex.Courses!.FindAsync(id);
                if (model.id != id || course == null)
                {
                    return NotFound();
                }
                var teacher = await _contex.Users!.FirstOrDefaultAsync(u => u.email == model.teacherEmail);
                if (teacher == null) { return BadRequest() ; }
                var role = await _contex.Roles.FirstOrDefaultAsync(r => r.id == teacher.roleId);
                if (teacher == null) { return NotFound(); }
                if(role == null || role.name != "Teacher" ) { return BadRequest(); }
                await _notificationRepository.AddNotification($"{course.courseName} updated at {DateTime.Now.ToLocalTime}", Int32.Parse(UserInfo()), false);
                await _repository.UpdateCourseAsync(id, model);
                return Ok("Update Successfully");
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("search-sort")]
        public async Task<IActionResult> SearchSort(string? search , string? course ,string?teacher , string?status)
        {
            try
            {
                return Ok(await _repository.SearchSort(search,course,teacher,status));
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet("filter")]
        public async Task<IActionResult> Filter()
        {
            try
            {
                return Ok(await _repository.Fillter());
            }
            catch
            {
                return BadRequest();
            }
        }




        public string UserInfo()
        {
            var result = _httpContextAccessor!.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier);
            return result;
        }
    }
}
