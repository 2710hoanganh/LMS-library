
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

        private string GetUserEmail()
        {
            var result =  string.Empty;
            if(_httpContextAccessor.HttpContext!=null)
            {
                result = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);
            }
            return result;
        }

        [HttpPost("upload-private-file")]
        public async Task<IActionResult> PostSingleFile(string name ,  [FromForm] PrivateFileUpload privateFileUpload  )
        {
            try
            {
                var result = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);
                var user = await _contex.Users.FirstOrDefaultAsync(u => u.email == result);
                var target = Path.Combine(_environment.ContentRootPath, "Private File");
                if(!Directory.Exists(target))
                {
                    Directory.CreateDirectory(target);
                }
                if (privateFileUpload != null)
                {

                    var filePath = Path.Combine(target, privateFileUpload.formFile.FileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await privateFileUpload.formFile.CopyToAsync(stream);
                    }
                    var newfile = new PrivateFiles
                    {
                        fileName = name,
                        fileType = Path.GetExtension(filePath),
                        fileSize = filePath.Length.ToString(),
                        filePath = filePath,
                        uploadAt = DateTime.Now,
                        updateAt = DateTime.Now,
                        userId = user.id,
                    };
                    _contex.PrivateFiles?.Add(newfile);
                    await _contex.SaveChangesAsync();
                }
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
