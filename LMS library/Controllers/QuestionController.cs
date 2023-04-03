using LMS_library.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LMS_library.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Teacher")]
    public class QuestionController : ControllerBase
    {

        private readonly IQuestionsRepository _repository;
        private readonly DataDBContex _contex;
        public QuestionController(IQuestionsRepository repository, DataDBContex contex)
        {
            _repository = repository;
            _contex = contex;
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
                if (model.id != id)
                {
                    return NotFound();
                }
                await _repository.UpdateQuestionAsync(id, model);
                return Ok("Update Successfully");
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpPost("excelfile")]
        public async Task<IActionResult> TestExcelfile(string courseName, string examName,string time, int examNumber, int pointRange, int questionQuantity, int easyQuestionQuantity, int nomalQuestionQuantity, int hardQuestionQuantity)
        {
            try
            {
                if(questionQuantity !=easyQuestionQuantity+ nomalQuestionQuantity +hardQuestionQuantity )
                {
                    return BadRequest("Wrong question quantity!");
                }

                await _repository.AddExamByExistQuestion(courseName,examName,time,examNumber,pointRange,questionQuantity,easyQuestionQuantity,nomalQuestionQuantity,hardQuestionQuantity);
                return Ok("Create Successfully");
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
