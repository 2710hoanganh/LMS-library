using DocumentFormat.OpenXml.Spreadsheet;
using LMS_library.Data;
using LMS_library.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LMS_library.Controllers
{
    [Route("api/[controller]")]
    [ApiController]


    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _repository;
        private readonly IUserEditRepository _userEditRepository;
        private readonly IPasswordRepository _passwordRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly DataDBContex _contex;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UsersController(INotificationRepository notificationRepository, IHttpContextAccessor httpContextAccessor, IUserRepository repository, DataDBContex contex, IUserEditRepository userEditRepository, IPasswordRepository passwordRepository)
        {
            _repository = repository;
            _contex = contex;
            _userEditRepository = userEditRepository;
            _httpContextAccessor = httpContextAccessor;
            _passwordRepository = passwordRepository;
            _notificationRepository = notificationRepository;
        }


        [HttpGet("list")]
        [Authorize(Roles = "Admin,Leader")]
        public async Task<IActionResult> GetAllUser()
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

        [HttpGet("search")]
        [Authorize(Roles = "Admin,Leader")]
        public async Task<IActionResult> Search(string? search)
        {
            try
            {
                return Ok(await _repository.Search(search));
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("filter-by-role")]
        [Authorize(Roles = "Admin,Leader")]
        public async Task<IActionResult> Filter(string? filter)
        {
            try
            {
                return Ok(await _repository.Filter(filter));
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Leader")]
        public async Task<IActionResult> GetUser(int id)
        {

            var user = await _repository.GetById(id);
            return user == null ? NotFound() : Ok(user);

        }

        [HttpPost("add-user")]
        [Authorize(Roles = "Admin,Leader")]
        public async Task<IActionResult> AddNewUser(UserModel model)
        {
            if (_contex.Users.Any(u => u.email == model.email))
            {
                return BadRequest("User already exists .");
            }
            await _notificationRepository.AddNotification($"{model.email} create successfully with role {model.role}",Int32.Parse(UserInfo()),false);
            var newUser = await _repository.AddUserAsync(model);
            return Ok(newUser);
        }


        [HttpDelete("delete/{id}")]
        [Authorize(Roles = "Admin,Leader")]
        public async Task<IActionResult> DeleteUser([FromRoute] int id)
        {
            var user = await _contex.Users.FindAsync(id);
            if (user == null) { return BadRequest(); }
            await _notificationRepository.AddNotification($"{user.email} has been deleted", Int32.Parse(UserInfo()), false);
            await _repository.DeleteUserAsync(id);
            return Ok("Delete Success !");

        }

        [HttpPut("update/{id}")]
        [Authorize(Roles = "Admin,Leader")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserEditModel model)
        {
            try
            {
                var user = await _contex.Users.FindAsync(id);
                if (model.id != id|| user == null)
                {
                    return NotFound();
                }
                await _notificationRepository.AddNotification($"Change detail {user.email} successfully!", Int32.Parse(UserInfo()), false);
                await _userEditRepository.UpdateUserAsync(id, model);
                return Ok("Update Successfully");
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPut("password-setting/{id}")]
        [Authorize(Roles = "Admin,Teacher,Student,Leader")]
        public async Task<IActionResult> ChangePassword(int id, [FromBody] Password model)
        {
            try
            {
                if (model.id != id)
                {
                    return NotFound();
                }
                await _notificationRepository.AddNotification($"Change password successfully!", Int32.Parse(UserInfo()), false);
                await _passwordRepository.ChangePassword(id, model);
                return Ok("Update Successfully");
            }
            catch
            {
                return BadRequest("Current Password Incorrect !");
            }
        }




        private string UserInfo()
        {
            var result = _httpContextAccessor!.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier);

            return result;
        }
        

    }
}
