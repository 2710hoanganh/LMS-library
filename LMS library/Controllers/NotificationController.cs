using LMS_library.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LMS_library.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly DataDBContex _contex;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public NotificationController( DataDBContex contex, IHttpContextAccessor httpContextAccessor, INotificationRepository notificationRepository)
        {

            _contex = contex;
            _httpContextAccessor = httpContextAccessor;
            _notificationRepository = notificationRepository;
        }


        [HttpGet("list")]
        public async Task<IActionResult> GetAllNotification()
        {
            try
            {
                return Ok(await _notificationRepository.GetAllById());
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetNotification(int id)
        {

            try
            {
                var notification = await _notificationRepository.GetById(id);
                return notification == null ? NotFound() : Ok(notification);
            }
            catch { return BadRequest(); }

        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {

            try
            {
               
                await _notificationRepository.DeleteNotificationAsync(id);
                return Ok("Delete Success !");

            }
            catch { return BadRequest(); }

        }

    }
}
