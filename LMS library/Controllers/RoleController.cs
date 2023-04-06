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
    [Authorize(Roles = "Admin,Leader")]
    public class RoleController : ControllerBase
    {
        private readonly IRoleRepository _repository;
        private readonly INotificationRepository _notificationRepository;
        private readonly DataDBContex _contex;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public RoleController(IRoleRepository repository, DataDBContex contex, IHttpContextAccessor httpContextAccessor,INotificationRepository notificationRepository)
        {

            _repository = repository;
            _contex = contex;
            _httpContextAccessor = httpContextAccessor;
            _notificationRepository= notificationRepository;
        }


        [HttpGet("list")]
        public async Task<IActionResult> GetAllRole()
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
        public async Task<IActionResult> GetRole(int id)
        {

            try
            {
                var role = await _repository.GetById(id);
                return role == null ? NotFound() : Ok(role);
            }
            catch { return BadRequest(); }

        }

        [HttpPost("add-role")]
        public async Task<IActionResult> AddNewRole( RoleModel model)
        {
            try
            {
                var result = _httpContextAccessor!.HttpContext!.User.FindFirstValue(ClaimTypes.Email);
                var user = await _contex.Users!.FirstOrDefaultAsync(u => u.email == result);
                if (_contex.Roles.Any(r => r.name == model.name))
                {
                    return BadRequest("Role already exists .");
                }
                var newNoti = new Notification
                {
                    message = $"Role {model.name} created successfully",
                    userId = user.id,
                    isRead = false,
                };
                var newRole = await _repository.AddRoleAsync(model);
                _contex.Notifications.Add(newNoti);
                await _contex.SaveChangesAsync();
                return Ok(newRole);
            }
            catch { return BadRequest(); }
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteRole([FromRoute] int id)
        {

            try
            {
                var user = await _contex.Users!.FirstOrDefaultAsync(r => r.roleId == id);
                if (user == null|| user.roleId == id )
                {
                    return BadRequest();
                }
                await _repository.DeleteRoleAsync(id);
                return Ok("Delete Success !");

            }
            catch { return BadRequest(); }

        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateRole(int id, [FromBody] RoleModel model)
        {
            try
            {
                var result = _httpContextAccessor!.HttpContext!.User.FindFirstValue(ClaimTypes.Email);
                var user = await _contex.Users!.FirstOrDefaultAsync(u => u.email == result);
                var role = await _contex.Roles!.FirstOrDefaultAsync(r => r.id == id);
                if (model.id != id||role ==null)
                {
                    return NotFound();
                }
                var newNoti = new Notification
                {
                    message = $"Change {role.name} to {model.name} successfully",
                    userId = user.id,
                    isRead = false,
                };
                _contex.Notifications.Add(newNoti);
                await _repository.UpdateRoleAsync(id, model);
                await _contex.SaveChangesAsync();
                return Ok("Update Successfully");
            }
            catch
            {
                return BadRequest();
            }
        }



        [HttpGet("search-role")]
        public async Task<IActionResult> Search(string? search)
        {
            try
            {
                return Ok(await _repository.Filter(search));
            }
            catch
            {
                return BadRequest();
            }
        }




    }
}
