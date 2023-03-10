using LMS_library.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LMS_library.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RoleController : ControllerBase
    {
        private readonly IRoleRepository _repository;
        private readonly DataDBContex _contex;
        public RoleController(IRoleRepository repository, DataDBContex contex)
        {
            _repository = repository;
            _contex = contex;
        }


        [HttpGet]
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

            var role = await _repository.GetById(id);
            return role == null ? NotFound() : Ok(role);

        }

        [HttpPost("add-role")]
        public async Task<IActionResult> AddNewUser(RoleModel model)
        {
            if (_contex.Roles.Any(r => r.name == model.name))
            {
                return BadRequest("Role already exists .");
            }
            var newRole = await _repository.AddRoleAsync(model);
            return Ok(newRole);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser([FromRoute] int id)
        {

            await _repository.DeleteRoleAsync(id);
            return Ok("Delete Success !");

        }
        //chua lam update nha
    }
}
