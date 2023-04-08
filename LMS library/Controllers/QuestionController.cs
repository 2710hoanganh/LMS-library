using LMS_library.Data;
using LMS_library.Migrations;
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
    public class QuestionController : ControllerBase
    {

        private readonly IQuestionsRepository _repository;
        private readonly INotificationRepository _notificationRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly DataDBContex _contex;
        public QuestionController(INotificationRepository notificationRepository, IHttpContextAccessor httpContextAccessor, IQuestionsRepository repository, DataDBContex contex)
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
        [HttpGet("{id}")]
        public async Task<IActionResult> GetQuestion(int id)
        {

            try
            {
                var question = await _repository.GetById(id);
                return question == null ? NotFound() : Ok(question);
            }
            catch { return BadRequest(); }

        }
        [HttpPost("add-question")]
        public async Task<IActionResult> AddNew(QuestionsModel model)
        {
            try
            {
                if (_contex.Questions.Any(r => r.questionName == model.questionName))
                {
                    return BadRequest("Question already exists .");
                }
                await _notificationRepository.AddNotification($"Create question for course {model.courseName} successfully at {DateTime.Now.ToLocalTime()}", Int32.Parse(UserInfo()), false);
                var newQuestion = await _repository.AddQuestionAsync(model);
                return Ok(newQuestion);
            }
            catch { return BadRequest(); }
        }
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {

            try
            {
                var question  = await _contex.Questions.FindAsync(id);
                if (question == null) { return NotFound(); }    
                await _notificationRepository.AddNotification($"Delete question of course {question.courseName} successfully at {DateTime.Now.ToLocalTime()}", Int32.Parse(UserInfo()), false);
                await _repository.DeleteQuestionAsync(id);
                return Ok("Delete Success !");

            }
            catch { return BadRequest(); }

        }
        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] QuestionsModel model)
        {
            try
            {
                var question = await _contex.Questions.FindAsync(id);
                if (model.id != id || question ==null)
                {
                    return NotFound();
                }
                await _notificationRepository.AddNotification($"Edit question {question.questionName} successfully at {DateTime.Now.ToLocalTime()}", Int32.Parse(UserInfo()), false);
                await _repository.UpdateQuestionAsync(id, model);
                return Ok("Update Successfully");
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpPost("excelfile")]
        public async Task<IActionResult> ExamFromExistQuestions(string courseName, string examName,string time, int examNumber, int pointRange, int questionQuantity, int easyQuestionQuantity, int nomalQuestionQuantity, int hardQuestionQuantity)
        {
            try
            {
                var leader = await _contex.Users.Where(l => l.Role.name == "Leader").ToListAsync();
                if (questionQuantity !=easyQuestionQuantity+ nomalQuestionQuantity +hardQuestionQuantity )
                {
                    return BadRequest("Wrong question quantity!");
                }
                foreach (var l in leader)
                {
                    await _notificationRepository.AddNotification($"New exam file create for {courseName} at {DateTime.Now.ToLocalTime()},exam name is {examName} please approve/reject the file soon as you can !", l.id, false);
                }
                await _notificationRepository.AddNotification($"Create exam by existing questions on system successfully at {DateTime.Now.ToLocalTime()} .Exam name is {examName}", Int32.Parse(UserInfo()), false);
                await _repository.AddExamByExistQuestion(courseName,examName,time,examNumber,pointRange,questionQuantity,easyQuestionQuantity,nomalQuestionQuantity,hardQuestionQuantity);
                return Ok("Create Successfully");
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
