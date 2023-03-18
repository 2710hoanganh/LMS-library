
using LMS_library.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
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

        [HttpGet("download-file/{id}")]
        public async Task<IActionResult> Download(int id)
        {
            try
            {
                var file = await _contex.PrivateFiles.FirstOrDefaultAsync(u => u.id == id);
                if (file == null)
                {
                    return NotFound();
                }
                // create a memorystream
                var memoryStream = new MemoryStream();

                using (var stream = new FileStream(file.filePath, FileMode.Open))
                {
                    await stream.CopyToAsync(memoryStream);
                }
                // set the position to return the file from
                memoryStream.Position = 0;
                
                return File(memoryStream, MimeTypes.GetMimeType(file.filePath), file.fileName);
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
