using Azure.Core;
using LMS_library.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;

namespace LMS_library.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
  

    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _repository;
        private readonly IUserEditRepository _userEditRepository;
        private readonly IPasswordRepository _passwordRepository;
        private readonly DataDBContex _contex;
        public UsersController(IUserRepository repository,DataDBContex contex, IUserEditRepository userEditRepository,IPasswordRepository passwordRepository)
        {
            _repository = repository;
            _contex = contex;
            _userEditRepository = userEditRepository;
            _passwordRepository = passwordRepository;
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
            if(_contex.Users.Any(u=>u.email== model.email))
            {
                return BadRequest("User already exists .");
            }
            var newUser = await _repository.AddUserAsync(model);    
            return Ok(newUser);
        }

 
        [HttpDelete("delete/{id}")]
        [Authorize(Roles = "Admin,Leader")]
        public async Task<IActionResult> DeleteUser([FromRoute]int id)
        {

            await _repository.DeleteUserAsync(id);
            return Ok("Delete Success !");

        }

        [HttpPut("update/{id}")]
        [Authorize(Roles = "Admin,Teacher,Student,Leader")]
        public async Task<IActionResult> UpdateUser(int id,[FromBody]UserEditModel model)
        {
            try
            {
                if (model.id != id)
                {
                    return NotFound();
                }
                await _userEditRepository.UpdateUserAsync(id, model);
                return Ok("Update Successfully");
            }
            catch
            {
                return BadRequest();
            }
        }
        
        [HttpPut("password-setting/{id}")]
        [Authorize(Roles ="Admin,Teacher,Student,Leader")]
        public async Task<IActionResult> ChangePassword(int id, [FromBody] Password model)
        {
            try
            {
                if (model.id != id)
                {
                    return NotFound();
                }
                await _passwordRepository.ChangePassword(id, model);
                return Ok("Update Successfully");
            }
            catch
            {
                return BadRequest("Current Password Incorrect !");
            }
        }

    }
}
