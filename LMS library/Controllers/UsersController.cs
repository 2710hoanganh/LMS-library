using LMS_library.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LMS_library.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _repository;
        private readonly DataDBContex _contex;
        public UsersController(IUserRepository repository , DataDBContex contex)
        {
            _repository = repository;
            _contex = contex;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            
            var user = await _repository.GetById(id);
            return user == null ? NotFound() : Ok(user);
           
        }
    }
}
