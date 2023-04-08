using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
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
        private readonly DataDBContex _contex;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly INotificationRepository _notificationRepository;
        public RoleController(INotificationRepository notificationRepository, IRoleRepository repository, DataDBContex contex, IHttpContextAccessor httpContextAccessor)
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
                if (_contex.Roles.Any(r => r.name == model.name))
                {
                    return BadRequest("Role already exists .");
                }
                await _notificationRepository.AddNotification($"Role {model.name} create successfully at {DateTime.Now.ToLocalTime}" ,Int32.Parse(UserInfo()), false);
                var newRole = await _repository.AddRoleAsync(model);
                return Ok(newRole);
            }
            catch { return BadRequest(); }
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteRole([FromRoute] int id)
        {

            try
            {

                var role = await _contex.Roles!.FindAsync(id);
                var user = await _contex.Users.ToListAsync();
                if ( user.Any(u => u.roleId == role.id))
                {
                    return BadRequest();
                }
                await _notificationRepository.AddNotification($"Role {role.name} deleted at {DateTime.Now.ToLocalTime()}", Int32.Parse(UserInfo()), false);

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
                var role = await _contex.Roles!.FirstOrDefaultAsync(r => r.id == id);
                if (model.id != id||role ==null)
                {
                    return NotFound();
                }
                await _notificationRepository.AddNotification($"Change {role.name} to {model.name} successfully", Int32.Parse(UserInfo()), false);
                await _repository.UpdateRoleAsync(id, model);
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

        private string UserInfo()
        {
            var result = _httpContextAccessor!.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier);
        
            return result;
        }
        

    }
}
