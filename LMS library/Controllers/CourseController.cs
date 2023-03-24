using LMS_library.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LMS_library.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,Leader, Teacher")]
    public class CourseController : ControllerBase
    {
        private readonly ICourseRepository _repository;
        private readonly DataDBContex _contex;
        public CourseController(ICourseRepository repository, DataDBContex contex)
        {
            _repository = repository;
            _contex = contex;
        }
        [Authorize(Roles = "Admin,Leader")]
        [HttpPost("add-course")]
        public async Task<IActionResult> AddNewRole(CourseModel model)
        {
            try
            {
                if (_contex.Courses.Any(r => r.courseName == model.courseName))
                {
                    return BadRequest("Course already exists .");
                }
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
                await _repository.DeleteCourseAsync(id);
                return Ok("Delete Success !");

            }
            catch { return BadRequest(); }

        }
        [Authorize(Roles = "Admin,Leader")]
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateCourse(int id, [FromBody] CourseModel model)
        {
            try
            {
                if (model.id != id)
                {
                    return NotFound();
                }
                var teacher = await _contex.Users.FirstOrDefaultAsync(u => u.email == model.teacherEmail);
                var role = await _contex.Roles.FirstOrDefaultAsync(r => r.id == teacher.roleId);
                if (teacher == null) { return NotFound(); }
                if(role == null || role.name != "Teacher" ) { return BadRequest(); }
                await _repository.UpdateCourseAsync(id, model);
                return Ok("Update Successfully");
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
