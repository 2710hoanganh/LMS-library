using LMS_library.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LMS_library.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Leader,Teacher")]
    public class TopicController : ControllerBase
    {
        private readonly IMaterialTopicRepository _repository;
        private readonly DataDBContex _contex;
        public TopicController(IMaterialTopicRepository repository, DataDBContex contex)
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
        public async Task<IActionResult> Get(int id)
        {

            try
            {
                var role = await _repository.GetById(id);
                return role == null ? NotFound() : Ok(role);
            }
            catch { return BadRequest(); }

        }

        [HttpPost("add-topic")]
        public async Task<IActionResult> AddNewTopic(MaterialTopicModel model)
        {
            try
            {
                if (_contex.Topics.Any(r => r.name == model.name))
                {
                    return BadRequest("Topic already exists .");
                }
                var newTopic = await _repository.AddTopicAsync(model);
                return Ok(newTopic);
            }
            catch { return BadRequest(); }
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {

            try
            {
                await _repository.DeleteTopicAsync(id);
                return Ok("Delete Success !");

            }
            catch { return BadRequest(); }

        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateRole(int id, [FromBody] MaterialTopicModel model)
        {
            try
            {
                if (model.id != id)
                {
                    return NotFound();
                }
                await _repository.UpdateTopicAsync(id, model);
                return Ok("Update Successfully");
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
