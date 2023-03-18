
using LMS_library.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.X509;
using System.Security.Claims;

namespace LMS_library.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PrivateFileController : ControllerBase
    {
        private readonly IPrivateFileRepository _repository;
        private readonly DataDBContex _contex;
        private IHostEnvironment _environment;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PrivateFileController(IPrivateFileRepository repository,DataDBContex contex, IHostEnvironment environment, IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _contex = contex;
            _environment = environment;
            _httpContextAccessor = httpContextAccessor;
        }


        [HttpPost("upload-file")]
        public async Task<IActionResult> PostMultiFile(List<IFormFile> privateFileUpload)
        {
            try
            {
                await _repository.PostMultiFileAsync(privateFileUpload);

                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

    }
}
