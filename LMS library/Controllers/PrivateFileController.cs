
using LMS_library.Data;
using LMS_library.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using Org.BouncyCastle.Asn1.X509;
using System.IO;
using System.Security.Claims;

namespace LMS_library.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,Leader")]
    public class PrivateFileController : ControllerBase
    {
        private readonly IPrivateFileRepository _repository;
        private readonly DataDBContex _contex;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly INotificationRepository _notificationRepository;

        public PrivateFileController(INotificationRepository notificationRepository, IHttpContextAccessor httpContextAccessor, IPrivateFileRepository repository,DataDBContex contex)
        {
            _repository = repository;
            _contex = contex;
            _httpContextAccessor = httpContextAccessor;
            _notificationRepository= notificationRepository;
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetAllFile()//get all private file
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
                var file = await _contex.PrivateFiles.FirstOrDefaultAsync(u => u.id == id);
                if (file == null){ return NotFound(); }

                System.IO.File.Delete(file.filePath);
                await _notificationRepository.AddNotification($"File {file.fileName} delete successfully at {DateTime.Now.ToLocalTime()}", Int32.Parse(UserInfo()), false);
                await _repository.DeleteFileAsync(id);
                return Ok();
            }
            catch { return BadRequest(); }

        }

        [HttpPost("upload-file")]
        public async Task<IActionResult> PostMultiFile(List<IFormFile> privateFileUpload)
        {
            try
            {
                await _notificationRepository.AddNotification($"Upload file private successfully at {DateTime.Now.ToLocalTime()}", Int32.Parse(UserInfo()), false);
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
                await _notificationRepository.AddNotification($"Download file {file.fileName} successfully at {DateTime.Now.ToLocalTime()}", Int32.Parse(UserInfo()), false);
                return File(memoryStream, MimeTypes.GetMimeType(file.filePath), file.fileName);
            }
            catch
            {
                return BadRequest();
            }



        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateRo(string newName ,int id)
        {
            try
            {
                var file = await _contex.PrivateFiles!.FirstOrDefaultAsync(u => u.id == id);
                if (file == null)
                {
                    return NotFound();
                }
                if(newName == null)
                {
                    return BadRequest("Please Enter File Name");
                }
                await _notificationRepository.AddNotification($"Change file {file.fileName} to {newName} successfully at {DateTime.Now.ToLocalTime()}", Int32.Parse(UserInfo()), false);
                await _repository.UpdateFileAsync(newName, id );

                return Ok("Update Successfully");
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("filter")]
        public async Task<IActionResult> Filter(string? type)//get all private file
        {
            try
            {
                return Ok(await _repository.Filter(type));
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet("search")]
        public async Task<IActionResult> Search(string? search)//get all private file
        {
            try
            {
                return Ok(await _repository.Search(search));
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
