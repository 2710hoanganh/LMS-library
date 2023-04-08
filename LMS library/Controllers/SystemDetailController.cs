using LMS_library.Data;
using LMS_library.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LMS_library.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,Leader")]
    public class SystemDetailController : ControllerBase
    {

        private readonly ISystemRepository _repository;
        private readonly DataDBContex _contex;
        private readonly INotificationRepository _notificationRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public SystemDetailController(INotificationRepository notificationRepository, IHttpContextAccessor httpContextAccessor, ISystemRepository repository, DataDBContex contex)
        {
            _repository = repository;
            _contex = contex;
            _notificationRepository = notificationRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost("add-system-detail")]
        public async Task<IActionResult> AddNewDetail(SystemModel model)
        {
            try
            {
                var count = await _contex.System.CountAsync();
                if (count == 1)
                {
                    return BadRequest("System detail already exists .");
                }
                await _notificationRepository.AddNotification($"System detail created successfully at {DateTime.Now.ToLocalTime()}", Int32.Parse(UserInfo()), false);
                var newUDetail = await _repository.AddDetailAsync(model);
                return Ok(newUDetail);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDetail(int id)
        {

            try
            {
                var detail = await _repository.GetById(id);
                return detail == null ? NotFound() : Ok(detail);
            }
            catch { return BadRequest(); }

        }
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteDtail([FromRoute] int id)
        {

            try
            {
                await _notificationRepository.AddNotification($"Delete system detail successfully at {DateTime.Now.ToLocalTime()}", Int32.Parse(UserInfo()), false);
                await _repository.DeleteDetailAsync(id);
                return Ok("Delete Success !");

            }
            catch { return BadRequest(); }

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDetail(int id, [FromBody] SystemModel model)
        {
            try
            {
                if (model.id != id)
                {
                    return NotFound();
                }
                await _notificationRepository.AddNotification($"Update system detail successfully at {DateTime.Now.ToLocalTime()}", Int32.Parse(UserInfo()), false);
                await _repository.UpdateDetaiAsync(id, model);
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
