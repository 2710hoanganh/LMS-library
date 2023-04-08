using LMS_library.Data;
using LMS_library.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LMS_library.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LessonController : ControllerBase
    {
        private readonly ILessonRepository _repository;
        private readonly DataDBContex _contex;
        private readonly INotificationRepository _notificationRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public LessonController(INotificationRepository notificationRepository, IHttpContextAccessor httpContextAccessor, ILessonRepository repository, DataDBContex contex)
        {
            _repository = repository;
            _contex = contex;
            _notificationRepository = notificationRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetAll()
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


        [HttpGet("list-on-topic")]
        public async Task<IActionResult> GetAllOnTopic(int id)
        {
            try
            {
                return Ok(await _repository.GetAllOnTopic(id));
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetLesson(int id)
        {

            try
            {
                var lesson = await _repository.GetById(id);
                return lesson == null ? NotFound() : Ok(lesson);
            }
            catch { return BadRequest(); }

        }


        [HttpPost("add-lesson")]
        public async Task<IActionResult> AddNew(LessonModel model)
        {
            try
            {
                if (_contex.Lessons.Any(r => r.name == model.name))
                {
                    return BadRequest("Lesson already exists .");
                }
                await _notificationRepository.AddNotification($"{model.name} create successfully at {DateTime.Now.ToLocalTime()}", Int32.Parse(UserInfo()), false);
                var newLesson = await _repository.AddLessonAsync(model);
                return Ok(newLesson);
            }
            catch { return BadRequest(); }
        }
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteRol([FromRoute] int id)
        {

            try
            {
                var lesson = await _contex.Lessons.FindAsync(id);
                await _notificationRepository.AddNotification($"{lesson.name} delete successfully at {DateTime.Now.ToLocalTime()}", Int32.Parse(UserInfo()), false);
                await _repository.DeleteLessonAsync(id);
                return Ok("Delete Success !");

            }
            catch { return BadRequest(); }

        }
        private string UserInfo()
        {
            var result = _httpContextAccessor!.HttpContext!.User.FindFirstValue(ClaimTypes.Name);
            return result;
        }

    }
}
