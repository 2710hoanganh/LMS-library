using AutoMapper;
using DocumentFormat.OpenXml.Office2010.Excel;
using System.Security.Claims;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Net.Mime.MediaTypeNames;

namespace LMS_library.Repositories
{
    public class CourseMaterialRepository : ICourseMaterialRepository
    {
        private readonly DataDBContex _contex;
        private IWebHostEnvironment _environment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;



        public CourseMaterialRepository(DataDBContex contex, IMapper mapper, IWebHostEnvironment environment, IHttpContextAccessor httpContextAccessor)
        {
            _contex = contex;
            _mapper = mapper;
            _environment = environment;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task PostMultiFileAsync(string type, string course, List<IFormFile> MaterialUploads)// type = material file type (Lesson or Resource) , course = course name
        {
            var result = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);
            if (result == null)
            {
                return;
            }
            int courseID = 0;
            var user = await _contex.Users.FirstOrDefaultAsync(u => u.email == result);
            var materialType = await _contex.MaterialTypes.FirstOrDefaultAsync(t => t.name == type);
            var courseName = await _contex.Courses.FirstOrDefaultAsync(c => c.courseName == course);
            if (courseName == null)
            {
                courseID = 0;
            }

            if (courseName != null)
            {
                courseID = courseName.id;

            }

            var target = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\System\Course Material");
            if (!Directory.Exists(target))
            {
                Directory.CreateDirectory(target);
            }

            foreach (var file in MaterialUploads)
            {
                if (file.Length <= 0) return;

                var filePath = Path.Combine(target, file.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                var newfile = new CourseMaterial
                {
                    name = file.FileName,
                    materialTypeID = materialType.id,
                    UserId = user.id,
                    courseId = courseID,
                    fileStatus = 0,
                    materialPath = filePath,
                    fileSize = filePath.Length,
                    submission_date = DateTime.Now,
                };
                _contex.Materials?.Add(newfile);
                await _contex.SaveChangesAsync();
            }
        }
        public async Task<List<CourseMaterial>> GetAllForStudent(int id)// course id
        {

            var files = await _contex.Materials!.Where(f => f.courseId == id && f.fileStatus == CourseMaterial.FileStatus.Approved)
                .ToListAsync();
            return _mapper.Map<List<CourseMaterial>>(files);
        }
        public async Task<List<CourseMaterial>> GetAll()// course id
        {

            var files = await _contex.Materials!
                .ToListAsync();
            return _mapper.Map<List<CourseMaterial>>(files);
        }

        public async Task<List<CourseMaterial>> GetAllBaseOnCourse(int id) //course id
        {

            var files = await _contex.Materials!
                .Where(f => f.courseId == id)
                .ToListAsync();
            return _mapper.Map<List<CourseMaterial>>(files);
        }

        public async Task<List<CourseMaterial>> GetAllForTeacher() //all material by teacher
        {
            var result = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var files = await _contex.Materials!
                .Where(f => f.UserId == Int32.Parse(result))
                .ToListAsync();
            return _mapper.Map<List<CourseMaterial>>(files);
        }
        
        public async Task<List<CourseMaterial>> GetAllLesson()
        {
            var result = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var files = await _contex.Materials!
                .Where(f => f.UserId == Int32.Parse(result) && f.MaterialType.Equals("Lesson"))
                .ToListAsync();
            return _mapper.Map<List<CourseMaterial>>(files);
        }

        public async Task<List<CourseMaterial>> GetAllResource() 
        {
            var result = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var files = await _contex.Materials!
                .Where(f => f.UserId == Int32.Parse(result) && f.MaterialType.Equals("Resource"))
                .ToListAsync();
            return _mapper.Map<List<CourseMaterial>>(files);
        }

        public async Task UpdateFileAsync(string newName, int id)// new file name and material id
        {
            var target = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\System\Course Material");
            var material = await _contex.Materials!.FindAsync(id);
            if (material == null) { return; }
            //file change Name
            string fileType = Path.GetExtension(material.materialPath);
            string FileName = newName + fileType;
            var filePath = Path.Combine(target, FileName);
            System.IO.File.Move(material.materialPath, filePath);

            material.name = FileName;
            material.materialTypeID = material.materialTypeID;
            material.materialPath = material.materialPath;
            material.fileSize = material.fileSize;
            material.submission_date = material.submission_date;
            material.fileStatus = material.fileStatus;
            material.courseId = material.courseId;
            material.UserId = material.UserId;
   
            _contex.Materials.Update(material);
            await _contex.SaveChangesAsync();
        }
        public async Task DeleteFileAsync(int id)
        {
            var deleteFile = await _contex.Materials.FindAsync(id);
            if (deleteFile.fileStatus == CourseMaterial.FileStatus.Approved) { return; }
            if (deleteFile != null)
            {
                _contex.Materials.Remove(deleteFile);
                await _contex.SaveChangesAsync();
            }
        }//matedial id
        public  async Task FileApprove(string check , int id) //check = file status (Approved or reject) , id = material id
        {
            var material = await _contex.Materials!.FindAsync(id);
            if(material != null)
            {
                var status  = CourseMaterial.FileStatus.Pendding;
    

                if(check == "Approved")
                {
                    status = CourseMaterial.FileStatus.Approved;
                }
                if(check == "Reject")
                {
                    status = CourseMaterial.FileStatus.Reject;
                }
                material.name = material.name;
                material.materialTypeID= material.materialTypeID;
                material.materialPath = material.materialPath;
                material.fileSize = material.fileSize;
                material.submission_date = material.submission_date;
                material.fileStatus = status;
                material.UserId = material.UserId;
                material.courseId = material.courseId;
                _contex.Materials.Update(material);
                await _contex.SaveChangesAsync();
            }
        }

        public async Task AddToResource(string lessonName, int id)
        {
            var material = await _contex.Materials!.FindAsync(id);
            var lesson = await _contex.Lessons!.FirstOrDefaultAsync(t => t.name == lessonName);
            var resource = await _contex.ResourceLists.FirstOrDefaultAsync(r => r.lessonId == lesson.id);
            if(lesson == null || material== null||resource ==null ||material.fileStatus == CourseMaterial.FileStatus.Reject || material.fileStatus == CourseMaterial.FileStatus.Pendding)
            {
                return;
            }

            material.name= material.name;
            material.materialTypeID= material.materialTypeID;
            material.materialPath = material.materialPath;  
            material.fileSize = material.fileSize;
            material.submission_date= material.submission_date;
            material.fileStatus= material.fileStatus;
            material.courseId = material.courseId;
            material.UserId = material.UserId;
            _contex.Materials.Update(material);
            await _contex.SaveChangesAsync();
        }

        public async Task<List<CourseMaterialModel>> SearchSort(string? course, string? teacher, string? status)
        {
            var material = _contex.Materials.AsQueryable(); 
            if(!string.IsNullOrEmpty(course) )
            {
                material = _contex.Materials.Where(c => c.courses.courseName.Contains(course));
            }
            if (!string.IsNullOrEmpty(teacher))
            {
                material = _contex.Materials.Where(c => c.User.email.Contains(teacher));
            }
            if (!string.IsNullOrEmpty(status))
            {
                var fileStatus = CourseMaterial.FileStatus.Pendding;
                if(status == "Pendding")
                {
                    fileStatus = CourseMaterial.FileStatus.Pendding;
                }
                if(status == "Approved")
                {
                    fileStatus = CourseMaterial.FileStatus.Approved;
                }
                if (status == "Reject")
                {
                    fileStatus = CourseMaterial.FileStatus.Reject;
                }
                material = _contex.Materials.Where(c => c.fileStatus.Equals(fileStatus));
            }
            var resutl = material.Select(r =>new CourseMaterialModel
            {
                id = r.id,
                name = r.name,
                materialType = r.MaterialType.name,
                teacherEmail = r.User.email,
                courseName = r.courses.courseName,
                fileStatus = r.fileStatus,
                materialPath = r.materialPath,
                fileSize = r.fileSize,
                submission_date= r.submission_date,

            });


            return resutl.ToList();

        }

        public async Task<List<CourseMaterialModel>> Fillter()
        {
            throw new NotImplementedException();
        }
    }
}
