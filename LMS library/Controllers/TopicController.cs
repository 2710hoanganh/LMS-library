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
    [Authorize(Roles = "Leader,Teacher")]
    public class TopicController : ControllerBase
    {
        private readonly IMaterialTopicRepository _repository;
        private readonly DataDBContex _contex;
        private readonly INotificationRepository _notificationRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public TopicController(INotificationRepository notificationRepository, IHttpContextAccessor httpContextAccessor, IMaterialTopicRepository repository, DataDBContex contex)
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
        public async Task<IActionResult> AddNewTopic(TopicModel model)
        {
            try
            {
                if (_contex.Topics.Any(r => r.name == model.name))
                {
                    return BadRequest("Topic already exists .");
                }
                await _notificationRepository.AddNotification($"Topic {model.name} create successfully at {DateTime.Now.ToLocalTime}", Int32.Parse(UserInfo()), false);

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
                var topic = await _contex.Topics!.FindAsync(id);
                if (topic == null) { return NotFound(); }
                await _notificationRepository.AddNotification($"Topic {topic.name} deleted at {DateTime.Now.ToLocalTime()}", Int32.Parse(UserInfo()), false);
                await _repository.DeleteTopicAsync(id);
                return Ok("Delete Success !");

            }
            catch { return BadRequest(); }

        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateTopic(int id, [FromBody] TopicModel model)
        {
            try
            {
                var topic = await _contex.Topics!.FindAsync(id);
                if (model.id != id|| topic == null)
                {
                    return NotFound();
                }
                await _notificationRepository.AddNotification($"Change {topic.name} to {model.name} successfully", Int32.Parse(UserInfo()), false);

                await _repository.UpdateTopicAsync(id, model);
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
