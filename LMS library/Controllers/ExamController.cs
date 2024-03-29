﻿using LMS_library.Data;
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
        private readonly INotificationRepository _notificationRepository;

        public ExamController(INotificationRepository notificationRepository, IExamRepository repository, DataDBContex contex, IHttpContextAccessor httpContextAccessor, IHostEnvironment environment)
        {
            _repository = repository;
            _contex = contex;
            _environment = environment;
            _httpContextAccessor = httpContextAccessor;
            _notificationRepository = notificationRepository;
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
                var leader = await _contex.Users.Where(l => l.Role.name == "Leader").ToListAsync();
                foreach (var l in leader)
                {
                    await _notificationRepository.AddNotification($"New exam file for {name} upload at {DateTime.Now.ToLocalTime()} please approve/reject the file soon as you can !", l.id, false);
                }
                await _notificationRepository.AddNotification($"Upload exam file for {name} successfully at {DateTime.Now.ToLocalTime()}", Int32.Parse(UserInfo()), false);
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
                var leader = await _contex.Users.Where(l => l.Role.name == "Leader").ToListAsync();
                foreach (var l in leader)
                {
                    await _notificationRepository.AddNotification($"New exam file for {name} upload at {DateTime.Now.ToLocalTime()} please approve/reject the file soon as you can !", l.id, false);
                }
                await _notificationRepository.AddNotification($"Upload exam file for {name} successfully at {DateTime.Now.ToLocalTime()}", Int32.Parse(UserInfo()), false);
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
                await _notificationRepository.AddNotification($"Download {file.fileName} successfully at {DateTime.Now.ToLocalTime()}", Int32.Parse(UserInfo()), false);

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
                await _notificationRepository.AddNotification($"Download {file.fileName} successfully at {DateTime.Now.ToLocalTime()}", Int32.Parse(UserInfo()), false);
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
                await _notificationRepository.AddNotification($"Change file name {file.fileName} to {newName} successfully at {DateTime.Now.ToLocalTime()}", Int32.Parse(UserInfo()), false);
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
                var teacher = await _contex.Users.FirstOrDefaultAsync(u => u.email == file.teacherEmail);
                if (check == "Approved")
                {
                    await _notificationRepository.AddNotification($"File {file.fileName} from {file.courseName} approved at {DateTime.Now.ToLocalTime()}", Int32.Parse(UserInfo()), false);
                    await _notificationRepository.AddNotification($"Your file {file.fileName} has been approved at {DateTime.Now.ToLocalTime()}", teacher.id, false);
                }
                if (check == "Reject")
                {
                    await _notificationRepository.AddNotification($"File {file.fileName} from {file.courseName} reject at {DateTime.Now.ToLocalTime()}", Int32.Parse(UserInfo()), false);
                    await _notificationRepository.AddNotification($"Your file {file.fileName} has been reject by leader at {DateTime.Now.ToLocalTime()}", teacher.id, false);
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
                var leader = await _contex.Users.Where(l => l.Role.name == "Leader").ToListAsync();
                foreach (var l in leader)
                {
                    await _notificationRepository.AddNotification($"New exam file {model.examName} create on system at {DateTime.Now.ToLocalTime()} please approve/reject the file soon as you can !", l.id, false);
                }
                await _notificationRepository.AddNotification($"Created exam file for {model.courseName} successfully at {DateTime.Now.ToLocalTime()}", Int32.Parse(UserInfo()), false);

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
                var leader = await _contex.Users.Where(l => l.Role.name == "Leader").ToListAsync();
                foreach (var l in leader)
                {
                    await _notificationRepository.AddNotification($"New exam file {model.examName} create on system at {DateTime.Now.ToLocalTime()} please approve/reject the file soon as you can !", l.id, false);
                }
                await _notificationRepository.AddNotification($"Created exam file for {model.courseName} successfully at {DateTime.Now.ToLocalTime()}", Int32.Parse(UserInfo()), false);

                await _repository.CreateEssayExamOnSystem(model);

                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpDelete("delete/{id}")]
        [Authorize(Roles = "Leader,Teacher")]
        public async Task<IActionResult> DeleteFile([FromRoute] int id) // file id 
        {
            try
            {
                var file = await _contex.Exams.FirstOrDefaultAsync(u => u.id == id);
                if (file == null) { return NotFound(); }

                System.IO.File.Delete(file.filePath);
                await _notificationRepository.AddNotification($"Exam {file.fileName} deleted at {DateTime.Now.ToLocalTime()}", Int32.Parse(UserInfo()), false);
                await _repository.DeleteExamAsyn(id);
                return Ok();
            }
            catch { return BadRequest(); }

        }

        private string UserInfo()
        {
            var result = _httpContextAccessor!.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier);
            return result;
        }
    }
}
