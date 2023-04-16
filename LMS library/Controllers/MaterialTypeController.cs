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
    [Authorize(Roles = "Admin,Leader,Teacher")]
    public class MaterialTypeController : ControllerBase
    {
        private readonly IMaterialTypeRepository _repository;
        private readonly DataDBContex _contex;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly INotificationRepository _notificationRepository;
        public MaterialTypeController(INotificationRepository notificationRepository, IHttpContextAccessor httpContextAccessor, IMaterialTypeRepository repository, DataDBContex contex)
        {
            _repository = repository;   
            _httpContextAccessor = httpContextAccessor;
            _notificationRepository = notificationRepository;
            _contex = contex;

        }


        [HttpGet("list")]
        public async Task<IActionResult> GetAll()//list file type 
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
        public async Task<IActionResult> GetMaterialType(int id)//get by id
        {

            try
            {
                var material_type = await _repository.GetById(id);
                return material_type == null ? NotFound() : Ok(material_type);
            }
            catch { return BadRequest(); }

        }
        [HttpPost("add-material-type")]
        public async Task<IActionResult> AddNewMaterialType(MaterialTypeModel model)//add new material type
        {
            try
            {
                if (_contex.MaterialTypes.Any(r => r.name == model.name))
                {
                    return BadRequest("Material type already exists .");
                }
                await _notificationRepository.AddNotification($"Material type {model.name} create successfully at {DateTime.Now.ToLocalTime()}", Int32.Parse(UserInfo()), false);
                var new_type = await _repository.AddMaterialTypeAsync(model);
                return Ok(new_type);
            }
            catch { return BadRequest(); }
        }


        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteMaterialType([FromRoute] int id)//delete by id
        {

            try
            {
                var type = await _contex.MaterialTypes.FindAsync(id);
                var material = await _contex.Materials!.FirstOrDefaultAsync(r => r.materialTypeID == id);
                if (material?.materialTypeID == id || type == null)
                {
                    return BadRequest();
                }
                await _notificationRepository.AddNotification($"Material type {type.name} delete successfully at {DateTime.Now.ToLocalTime()}", Int32.Parse(UserInfo()), false);
                await _repository.DeleteMaterialTypeAsync(id);
                return Ok("Delete Success !");

            }
            catch { return BadRequest(); }

        }


        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateMaterialType(int id, [FromBody] MaterialTypeModel model)//update by id
        {
            try
            {
                var type = await _contex.MaterialTypes.FindAsync(id);
                if (model.id != id)
                {
                    return NotFound();
                }
                await _notificationRepository.AddNotification($"Change material type {type.name} to {model.name} successfully at {DateTime.Now.ToLocalTime()}", Int32.Parse(UserInfo()), false);
                await _repository.UpdateMaterialTypeAsync(id, model);
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
