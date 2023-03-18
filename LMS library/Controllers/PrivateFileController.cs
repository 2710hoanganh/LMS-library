
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

        public PrivateFileController(IPrivateFileRepository repository,DataDBContex contex)
        {
            _repository = repository;
            _contex = contex;
        }
        [HttpGet("list")]
        public async Task<IActionResult> GetAllFile()
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

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteFile([FromRoute] int id)
        {

            try
            {
                await _repository.DeleteFileAsync(id);
                return Ok("Delete Success !");

            }
            catch { return BadRequest(); }

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
