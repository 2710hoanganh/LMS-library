using LMS_library.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MimeKit;

namespace LMS_library.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class CourseMaterialController : ControllerBase
    {

        private readonly ICourseMaterialRepository _repository;
        private readonly DataDBContex _contex;

        public CourseMaterialController(ICourseMaterialRepository repository, DataDBContex contex)
        {
            _repository = repository;
            _contex = contex;
        }


        [Authorize(Roles = "Leader,Teacher")]
        [HttpPost("upload-file")]
        public async Task<IActionResult> PostMultiFile( string type , string course , List<IFormFile> FileUpload)//upload lesson or resourse , type = file type (lesson or resourse) , course = course name
        {
            try
            {
                await _repository.PostMultiFileAsync( type , course, FileUpload);

                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        [Authorize(Roles = "Leader,Student,Teacher")]
        [HttpGet("approved-list-student")]
        public async Task<IActionResult> GetAllFileForStudent(int id)//course id  , student cant see the pendding or reject file ,ONLY SEE THE FILE BEEN APPROVED
        {
            try
            {
                return Ok(await _repository.GetAllForStudent(id));
            }
            catch
            {
                return BadRequest();
            }
        }

        [Authorize(Roles = "Leader,Student,Teacher")]
        [HttpGet("material/list")]
        public async Task<IActionResult> GetAll(int id)//course id , get all material (lesson or resourse) belong to the course 
        {
            try
            {
                return Ok(await _repository.GetAll(id));
            }
            catch
            {
                return BadRequest();
            }
        }

        [Authorize(Roles = "Leader")]
        [HttpPut("approve/{id}")]
        public async Task<IActionResult> FileApprove(string check,int id) // check = file status (approved or reject),id = file id (lesson or resourse) ,leader can approved or rejected the file 
        {
            try
            {
                var file = await _contex.Materials!.FirstOrDefaultAsync(u => u.id == id);
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

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteFile([FromRoute] int id) // file id 
        {
            try
            {
                var file = await _contex.Materials.FirstOrDefaultAsync(u => u.id == id);
                if (file == null) { return NotFound(); }

                System.IO.File.Delete(file.materialPath);

                await _repository.DeleteFileAsync(id);
                return Ok();
            }
            catch { return BadRequest(); }

        }


        [Authorize(Roles = "Leader,Teacher")]
        [HttpPut("update-file-name/{id}")]
        public async Task<IActionResult> UpdateName(string newName, int id)// change file name , newName = new file name , id = file id
        {
            try
            {
                var file = await _contex.Materials!.FirstOrDefaultAsync(u => u.id == id);
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

        [Authorize(Roles = "Leader,Teacher")]
        [HttpPut("add-to-topic/{id}")]
        public async Task<IActionResult> AddToTopic(string topic, int id)// change file name , newName = new file name , id = file id
        {
            try
            {
                var file = await _contex.Materials!.FirstOrDefaultAsync(u => u.id == id);
                if (file == null)
                {
                    return NotFound();
                }
                if (topic == null)
                {
                    return BadRequest("Please Enter File Name");
                }

                await _repository.AddToTopic(topic, id);

                return Ok("Update Successfully");
            }
            catch
            {
                return BadRequest();
            }
        }


        [HttpGet("download-file/{id}")]
        [Authorize(Roles = "Leader,Teacher")]
        public async Task<IActionResult> Download(int id)// download file by file id
        {
            try
            {
                var file = await _contex.Materials.FirstOrDefaultAsync(u => u.id == id);
                if (file == null)
                {
                    return NotFound();
                }
                // create a memorystream
                var memoryStream = new MemoryStream();

                using (var stream = new FileStream(file.materialPath, FileMode.Open))
                {
                    await stream.CopyToAsync(memoryStream);
                }
                // set the position to return the file from
                memoryStream.Position = 0;

                return File(memoryStream, MimeTypes.GetMimeType(file.materialPath), file.materialPath);
            }
            catch
            {
                return BadRequest();
            }



        }

    }
}
