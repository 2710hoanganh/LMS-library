using LMS_library.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace LMS_library.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SystemDetailController : ControllerBase
    {

        private readonly ISystemRepository _repository;
        private readonly DataDBContex _contex;
        public SystemDetailController(ISystemRepository repository, DataDBContex contex)
        {
            _repository = repository;
            _contex = contex;
        }



        [HttpPost("add-system-detail")]
        public async Task<IActionResult> AddNewDetail(SystemModel model)
        {
            try
            {
                if (_contex.System.Any(u => u.id == 1))
                {
                    return BadRequest("System detail already exists .");
                }

                var newUDetail = await _repository.AddDetailAsync(model);
                return Ok(newUDetail);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDetail(int id)
        {

            try
            {
                var detail = await _repository.GetById(id);
                return detail == null ? NotFound() : Ok(detail);
            }
            catch { return BadRequest(); }

        }
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteDtail([FromRoute] int id)
        {

            try
            {
                await _repository.DeleteDetailAsync(id);
                return Ok("Delete Success !");

            }
            catch { return BadRequest(); }

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDetail(int id, [FromBody] SystemModel model)
        {
            try
            {
                if (model.id != id)
                {
                    return NotFound();
                }
                await _repository.UpdateDetaiAsync(id, model);
                return Ok("Update Successfully");
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
