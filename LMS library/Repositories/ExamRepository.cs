using AutoMapper;
using System.Security.Claims;
using ExcelDataReader;
using System.Data;
using OfficeOpenXml;
using Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using ClosedXML.Excel;
using System.Drawing.Printing;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Spreadsheet;
using System.IO;
using Xceed.Words.NET;

namespace LMS_library.Repositories
{
    public class ExamRepository : IExamRepository
    {
        private readonly DataDBContex _contex;
        private IWebHostEnvironment _environment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public ExamRepository(DataDBContex contex, IMapper mapper, IWebHostEnvironment environment, IHttpContextAccessor httpContextAccessor)
        {
            _contex = contex;
            _mapper = mapper;
            _environment = environment;
            _httpContextAccessor = httpContextAccessor;
        }


        public async Task PostWordAsync(string name, string time, List<IFormFile> privateFileUploads)
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
            var target = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\System\Exam");
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
                    create_At = DateTime.Now,
                };
                _contex.Exams?.Add(newfile);
                await _contex.SaveChangesAsync();
            }
        }

        public async Task PostExcelAsync(string name, string time, List<IFormFile> privateFileUploads)
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
            var target = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\System\Exam");
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

            var files = await _contex.Exams!.Where(f => f.courseName == name && f.examStatus == Exam.ExamStatus.Approved)
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
            if (file.examStatus == Exam.ExamStatus.Pendding || file.examStatus == Exam.ExamStatus.Reject) { return; }
            //file change Name
            string fileType = Path.GetExtension(file.filePath);
            string FileName = newName + fileType;
            var filePath = Path.Combine(target, FileName);
            System.IO.File.Move(file.filePath, filePath);

            file.fileType = file.fileType;
            file.fileName = FileName;
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

        public async Task CreateMultiChoiseExamOnSystem(MultiChoiseExamModel model)
        {
            var result = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);
            var course = await _contex.Courses.FirstOrDefaultAsync(c => c.courseName == model.courseName);
            if (result == null || course == null )
            {
                return ;
            }
            var target = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\System\Exam");
            if (!Directory.Exists(target))
            {
                Directory.CreateDirectory(target);
            }

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add(model.examName);

                worksheet.Cell(1, 1).Value = $"Course Name : {model.courseName}";
                worksheet.Cell(1, 2).Value = $"Exam Name : {model.examName}";

                worksheet.Cell(2, 1).Value = "Question Name";
                worksheet.Cell(2, 2).Value = "Answer A";
                worksheet.Cell(2, 3).Value = "Answer B";
                worksheet.Cell(2, 4).Value = "Answer C";
                worksheet.Cell(2, 5).Value = "Answer D";
                worksheet.Cell(2, 6).Value = "Correct Answer";

                worksheet.Column(1).Width = 20;
                worksheet.Column(2).Width = 20;
                worksheet.Column(3).Width = 20;
                worksheet.Column(4).Width = 20;
                worksheet.Column(5).Width = 20;
                worksheet.Column(6).Width = 40;

                // Write the data
                var row = 3;
                foreach (var question in model.questions)
                {
                    worksheet.Cell(row, 1).Value = question.questionName;
                    worksheet.Cell(row, 2).Value = question.answerA;
                    worksheet.Cell(row, 3).Value = question.answerB;
                    worksheet.Cell(row, 4).Value = question.answerC;
                    worksheet.Cell(row, 5).Value = question.answerD;
                    worksheet.Cell(row, 6).Value = question.correctAnswer;
                    row++;
                }
                //Convert ExcelPackage to Byte array
                var filePath = Path.Combine(target, $"{model.examName}.xlsx");
                var filestream = new FileStream(filePath, FileMode.Create);
                workbook.SaveAs(filestream);
                filestream.Close();
                var newfile = new Exam
                {
                    fileType = Path.GetExtension(filePath),
                    fileName = $"{model.examName}.xlsx",
                    filePath = filePath,
                    courseName = model.courseName,
                    teacherEmail = result,
                    examType = Exam.ExamType.Selected,
                    time = model.time,
                    examStatus = Exam.ExamStatus.Draft,
                    create_At = DateTime.Now,
                };
                _contex.Exams?.Add(newfile);
                await _contex.SaveChangesAsync();
            }
        }


        public async Task CreateEssayExamOnSystem(EssayExamModel model)
        {
            var result = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);
            var course = await _contex.Courses.FirstOrDefaultAsync(c => c.courseName == model.courseName);
            if (result == null || course == null )
            {
                return;
            }
            var target = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\System\Exam");
            if (!Directory.Exists(target))
            {
                Directory.CreateDirectory(target);
            }
            var filePath = Path.Combine(target, $"{model.examName}.docx");
            // Create a new WordprocessingDocument
            using (DocX doc = DocX.Create(filePath))
            {
                doc.InsertParagraph($"Exam Name : {model.examName}").AppendLine();
                doc.InsertParagraph($"Course Name : {model.courseName}").AppendLine();
                doc.InsertParagraph($"Time : {model.time}").AppendLine();
                int i = 1;
                foreach (var question in model.questions)
                {
                    doc.InsertParagraph($"Question {i++} : {question.questionName}").AppendLine();
                    doc.InsertParagraph($"Question Answer : {question.questionAnswer}").AppendLine();
                }
                var filestream = new FileStream(filePath, FileMode.Create);
                doc.SaveAs(filestream);

                filestream.Close();


                var newfile = new Exam
                {
                    fileType = Path.GetExtension(filePath),
                    fileName = $"{model.examName}.docx",
                    filePath = filePath,
                    courseName = model.courseName,
                    teacherEmail = result,
                    examType = Exam.ExamType.Contructed,
                    time = model.time,
                    examStatus = Exam.ExamStatus.Draft,
                    create_At = DateTime.Now,
                };
                _contex.Exams?.Add(newfile);
                await _contex.SaveChangesAsync();

            }

        }

        public async Task DeleteExamAsyn(int id)
        {
            var exam = await _contex.Exams!.FindAsync(id);
            if (exam == null) { return; }
            _contex.Exams?.Remove(exam);
            await _contex.SaveChangesAsync();
        }
    }
}
