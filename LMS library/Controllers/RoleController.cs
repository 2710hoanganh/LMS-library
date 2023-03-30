using LMS_library.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LMS_library.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,Leader")]
    public class RoleController : ControllerBase
    {
        private readonly IRoleRepository _repository;
        private readonly DataDBContex _contex;
        public RoleController(IRoleRepository repository, DataDBContex contex)
        {
            _repository = repository;
            _contex = contex;
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
        public async Task<IActionResult> AddNewRole(RoleModel model)
        {
            try
            {
                if (_contex.Roles.Any(r => r.name == model.name))
                {
                    return BadRequest("Role already exists .");
                }
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
                if (model.id != id)
                {
                    return NotFound();
                }
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




    }
}
