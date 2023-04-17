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
    [Authorize(Roles = "Teacher")]
    public class LessonController : ControllerBase
    {
        private readonly ILessonRepository _repository;
        private readonly ILessonQuestionRepository _lessonQuestionRepository;
        private readonly IAnswerRepository _answerRepository;
        private readonly DataDBContex _contex;
        private readonly INotificationRepository _notificationRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public LessonController(ILessonQuestionRepository lessonQuestionRepository,IAnswerRepository answerRepository,INotificationRepository notificationRepository, IHttpContextAccessor httpContextAccessor, ILessonRepository repository, DataDBContex contex)
        {
            _repository = repository;
            _contex = contex;
            _notificationRepository = notificationRepository;
            _httpContextAccessor = httpContextAccessor;
            _lessonQuestionRepository= lessonQuestionRepository;
            _answerRepository= answerRepository;
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


        [Authorize(Roles = "Teacher,Student")]
        [HttpPost("add-lesson-question")]
        public async Task<IActionResult> AddQuestions(LessonQuestionModel model)
        {
            try
            {
                await _notificationRepository.AddNotification($"Question {model.title} sent successfully at {DateTime.Now.ToLocalTime()}", Int32.Parse(UserInfo()), false);                var newQuestion = await _lessonQuestionRepository.AddQuestion(model);
                return Ok(newQuestion);
            }
            catch { return BadRequest(); }
        }
        [Authorize(Roles = "Teacher,Student")]
        [HttpPost("answer-lesson-question")]
        public async Task<IActionResult> AnswerQuestion(AnswerModel model)
        {
            try
            {
                var question = await _contex.LessonQuestions!.FindAsync(model.questionId);
                if (question == null) { return NotFound(); }
                await _notificationRepository.AddNotification($"Your answer for {question.title} sent successfully at {DateTime.Now.ToLocalTime()}", Int32.Parse(UserInfo()), false);
                await _notificationRepository.AddNotification($"Your question has answered by {Int32.Parse(UserInfo())} at {DateTime.Now.ToLocalTime()}" , question.userId,false);
                var newAnswer = await _answerRepository.AnswerQuestion(model);
                return Ok(newAnswer);
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
                await _notificationRepository.AddNotification($"New lesson {model.name} create successfully at {DateTime.Now.ToLocalTime()}", Int32.Parse(UserInfo()), false);
                var newLesson = await _repository.AddLessonAsync(model);
                return Ok(newLesson);
            }
            catch { return BadRequest(); }
        }
        [HttpPost("add-lesson-and-upload-file")]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> AddLessonAndUploadFile(string course , string topic , string title , IFormFile formFile)
        {
            try
            {
                if (_contex.Lessons.Any(r => r.name == title))
                {
                    return BadRequest("Lesson already exists .");
                }
                await _notificationRepository.AddNotification($"New lesson {title} create successfully and upload {formFile.FileName} for review successfully at {DateTime.Now.ToLocalTime()}", Int32.Parse(UserInfo()), false);
                var newLesson = await _repository.AddLessonAndUploadFileAsync(course , topic,title,formFile);
                return Ok(newLesson);
            }
            catch { return BadRequest(); }
        }
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {

            try
            {
                var lesson = await _contex.Lessons.FindAsync(id);
                if (lesson == null) {return BadRequest();}  
                await _notificationRepository.AddNotification($"Lesson {lesson.name} delete successfully at {DateTime.Now.ToLocalTime()}", Int32.Parse(UserInfo()), false);
                await _repository.DeleteLessonAsync(id);
                return Ok("Delete Success !");

            }
            catch { return BadRequest(); }

        }
        private string UserInfo()
        {
            var result = _httpContextAccessor!.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier);
            return result;
        }

    }
}
