using AutoMapper;
using System.Security.Claims;
using ExcelDataReader;
using System.Data;
using OfficeOpenXml;
using Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;


namespace LMS_library.Repositories
{
    public class ExamRepository : IExamRepository
    {
        private readonly DataDBContex _contex;
        private IHostEnvironment _environment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public ExamRepository(DataDBContex contex, IMapper mapper, IHostEnvironment environment, IHttpContextAccessor httpContextAccessor)
        {
            _contex = contex;
            _mapper = mapper;
            _environment = environment;
            _httpContextAccessor = httpContextAccessor;
        }


        public async Task PostWordAsync(string name,string time , List<IFormFile> privateFileUploads)
        {
            var result = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);
            if (result == null)
            {
                return;
            }
            var user = await _contex.Users.FirstOrDefaultAsync(u => u.email == result);
            var course = await _contex.Courses.FirstOrDefaultAsync(c => c.courseName == name);
            if(course.userId != user.id)
            {
                return;
            }
            var target = Path.Combine(_environment.ContentRootPath, "Exam File");
            if (!Directory.Exists(target))
            {
                Directory.CreateDirectory(target);
            }

            foreach (var file in privateFileUploads)
            {
                if (file.Length <= 0) return;

                var filePath = Path.Combine(target, file.FileName);
                
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                var newfile = new Exam
                {
                    fileType = Path.GetExtension(filePath),
                    fileName = file.FileName,
                    filePath = filePath,
                    courseName = name,
                    teacherEmail = result,
                    examType = Exam.ExamType.Contructed,
                    time = time,
                    examStatus = Exam.ExamStatus.Draft,
                    create_At= DateTime.Now,
                };
                _contex.Exams?.Add(newfile);
                await _contex.SaveChangesAsync();
            }
        }

        public async Task PostExcelAsync(string name,string time, List<IFormFile> privateFileUploads)
        {
            var result = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);
            if (result == null)
            {
                return;
            }
            var user = await _contex.Users.FirstOrDefaultAsync(u => u.email == result);
            var course = await _contex.Courses.FirstOrDefaultAsync(c => c.courseName == name);
            if (course.userId != user.id)
            {
                return;
            }
            var target = Path.Combine(_environment.ContentRootPath, "Exam File");
            if (!Directory.Exists(target))
            {
                Directory.CreateDirectory(target);
            }

            foreach (var file in privateFileUploads)
            {

                if (file.Length <= 0) return;
                var filePath = Path.Combine(target, file.FileName);
                if (Path.GetExtension(filePath) != ".xlsx") { return; }
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var newfile = new Exam
                {
                    fileType = Path.GetExtension(filePath),
                    fileName = file.FileName,
                    filePath = filePath,
                    courseName = name,
                    teacherEmail = result,
                    examType = Exam.ExamType.Selected,
                    time = time,
                    examStatus = Exam.ExamStatus.Draft,
                    create_At = DateTime.Now,
                };
                _contex.Exams?.Add(newfile);
                await _contex.SaveChangesAsync();
            }
        }

        public async Task<List<Exam>> GetAllForStudent(string name)// course name
        {

            var files = await _contex.Exams!.Where(f => f.courseName == name &&f.examStatus == Exam.ExamStatus.Approved)
                .ToListAsync();
            return _mapper.Map<List<Exam>>(files);
        }
        public async Task<List<Exam>> GetAll(string name) //course name

        {

            var files = await _contex.Exams!
                .Where(f => f.courseName == name)
                .ToListAsync();
            return _mapper.Map<List<Exam>>(files);
        }

        public async Task UpdateFileAsync(string newName, int id)// new file name and material id
        {
            var target = Path.Combine(_environment.ContentRootPath, "Exam File");
            var file = await _contex.Exams!.FindAsync(id);
            if (file == null) { return; }
            if(file.examStatus == Exam.ExamStatus.Pendding || file.examStatus == Exam.ExamStatus.Reject) { return; }
            //file change Name
            string fileType = Path.GetExtension(file.filePath);
            string FileName = newName + fileType;
            var filePath = Path.Combine(target, FileName);
            System.IO.File.Move(file.filePath, filePath);

            file.fileType = file.fileType;
            file.fileName= FileName;
            file.filePath = filePath;
            file.courseName = file.courseName;
            file.teacherEmail = file.teacherEmail;
            file.examType = file.examType;
            file.time = file.time;
            file.examStatus = file.examStatus;
            file.create_At = file.create_At;

            _contex.Exams.Update(file);
            await _contex.SaveChangesAsync();
        }
        public async Task FileApprove(string check, int id) //check = file status (Approved or reject) , id = material id
        {
            var file = await _contex.Exams!.FindAsync(id);
            if (file != null)
            {
                var status = Exam.ExamStatus.Pendding;


                if (check == "Approved")
                {
                    status = Exam.ExamStatus.Approved;
                }
                if (check == "Reject")
                {
                    status = Exam.ExamStatus.Reject;
                }
                file.fileType = file.fileType;
                file.fileName = file.fileName;
                file.filePath = file.filePath;
                file.courseName = file.courseName;
                file.teacherEmail = file.teacherEmail;
                file.examType = file.examType;
                file.time = file.time;
                file.examStatus = status;
                file.create_At = file.create_At;
                _contex.Exams.Update(file);
                await _contex.SaveChangesAsync();
            }
        }

    }
}
