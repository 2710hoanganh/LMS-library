using Azure.Core;
using LMS_library.Repositories;
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
        private readonly DataDBContex _contex;
        public UsersController(IUserRepository repository,DataDBContex contex)
        {
            _repository = repository;
            _contex = contex;
        }

        [HttpGet]
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
        public async Task<IActionResult> GetUser(int id)
        {
            
            var user = await _repository.GetById(id);
            return user == null ? NotFound() : Ok(user);
           
        }

        [HttpPost("add-user")]
        public async Task<IActionResult> AddNewUser(UserModel model)
        {
            if(_contex.Users.Any(u=>u.email== model.email))
            {
                return BadRequest("User already exists .");
            }

            var newUser = await _repository.AddUserAsync(model);    
            return Ok(newUser);
        }















   
    }
}
