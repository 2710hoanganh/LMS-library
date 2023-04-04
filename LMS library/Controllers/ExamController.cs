using LMS_library.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using System;
using System.Security.Claims;

namespace LMS_library.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ExamController : ControllerBase
    {
        private readonly IExamRepository _repository;
        private readonly DataDBContex _contex;
        private IHostEnvironment _environment;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ExamController(IExamRepository repository, DataDBContex contex, IHttpContextAccessor httpContextAccessor, IHostEnvironment environment)
        {
            _repository = repository;
            _contex = contex;
            _environment = environment;
            _httpContextAccessor = httpContextAccessor;
        }

        [Authorize(Roles = "Leader,Student,Teacher")]
        [HttpGet("approved-list-student")]
        public async Task<IActionResult> GetAllFileForStudent(string name)//ONLY SEE THE FILE BEEN APPROVED
        {
            try
            {
                return Ok(await _repository.GetAllForStudent(name));
            }
            catch
            {
                return BadRequest();
            }
        }
        [Authorize(Roles = "Leader,Teacher")]
        [HttpGet("exam/list")]
        public async Task<IActionResult> GetAll(string name)//, get all material (lesson or resourse) belong to the course 
        {
            try
            {
                return Ok(await _repository.GetAll(name));
            }
            catch
            {
                return BadRequest();
            }
        }

        [Authorize(Roles = "Leader,Teacher")]
        [HttpPost("upload-word-file")]
        public async Task<IActionResult> PostWordFile(string name, string time ,List<IFormFile> privateFileUpload)
        {
            try
            {
                await _repository.PostWordAsync(name ,time,privateFileUpload);

                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }
        [Authorize(Roles = "Leader,Teacher")]
        [HttpPost("upload-excel-file")]
        public async Task<IActionResult> PostExcelFile(string name, string time, List<IFormFile> privateFileUpload)
        {
            try
            {
                await _repository.PostExcelAsync(name, time, privateFileUpload);

                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        [Authorize(Roles = "Leader,Student,Teacher")]
        [HttpGet("download-file-for-student/{id}")]
        public async Task<IActionResult> DownloadForStudent(int id)
        {
            try
            {
                var file = await _contex.Exams.FirstOrDefaultAsync(u => u.id == id);
                if (file == null)
                {
                    return NotFound();
                }
                if (file.examStatus != Exam.ExamStatus.Approved)
                {
                    return BadRequest();
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
        [Authorize(Roles = "Leader,Teacher")]
        [HttpGet("download-file/{id}")]
        public async Task<IActionResult> Download(int id)
        {
            try
            {
                var file = await _contex.Exams.FirstOrDefaultAsync(u => u.id == id);
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

        [Authorize(Roles = "Leader,Teacher")]
        [HttpPut("update-file-name/{id}")]
        public async Task<IActionResult> UpdateName(string newName, int id)// change file name , newName = new file name , id = file id
        {
            try
            {
                var file = await _contex.Exams!.FirstOrDefaultAsync(u => u.id == id);
                var result = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);
                if (result == null)
                {
                    return BadRequest();
                }
                if (file == null)
                {
                    return NotFound();
                }
                if (newName == null)
                {
                    return BadRequest("Please Enter File Name");
                }

                await _repository.UpdateFileAsync(newName, id);

                return Ok("Update Successfully");
            }
            catch
            {
                return BadRequest();
            }
        }

        [Authorize(Roles = "Leader")]
        [HttpPut("approve/{id}")]
        public async Task<IActionResult> FileApprove(string check, int id) // check = file status (approved or reject),id = file id (lesson or resourse) ,leader can approved or rejected the file 
        {
            try
            {
                var file = await _contex.Exams!.FirstOrDefaultAsync(u => u.id == id);
                if (file == null)
                {
                    return NotFound();
                }
                if (check == null)
                {
                    return BadRequest("Please Enter Choose Approve Or Reject File");
                }

                await _repository.FileApprove(check, id);

                return Ok("Update Successfully");
            }
            catch
            {
                return BadRequest();
            }
        }
        [Authorize(Roles = "Leader,Teacher")]
        [HttpPost("create-multi-choise-exam-on-system")]
        public async Task<IActionResult> CreateMultiCHoiseExam([FromBody] MultiChoiseExamModel model)
        {
            try
            {
                
                await _repository.CreateMultiChoiseExamOnSystem(model);

                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }
        [Authorize(Roles = "Leader,Teacher")]
        [HttpPost("create-essy-exam-on-system")]
        public async Task<IActionResult> CreateEssayExam([FromBody] EssayExamModel model)
        {
            try
            {

                await _repository.CreateEssayExamOnSystem(model);

                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
