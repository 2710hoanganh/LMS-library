using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Office.Interop.Excel;
using OfficeOpenXml;
using System;
using System.IO;
using ClosedXML;
using System.Security.Claims;
using ClosedXML.Excel;
using Microsoft.Office.Interop.Excel;
using System.IO.Packaging;
using System.Linq;
using static LMS_library.Data.MultipleChoiceQuestions;
using LMS_library.Models;

namespace LMS_library.Repositories
{
    public class QuestionsRepository : IQuestionsRepository
    {


        private readonly IMapper _mapper;
        private readonly DataDBContex _contex;
        private IWebHostEnvironment _environment;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public QuestionsRepository(IMapper mapper, DataDBContex contex, IHttpContextAccessor httpContextAccessor , IWebHostEnvironment environment)
        {
            _mapper = mapper;
            _contex = contex;
            _environment = environment;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<string> AddQuestionAsync(QuestionsModel model)
        {

            var result = _httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.Email);
            if (result == null)
            {
                return ("User Is Required");
            }
            var level = MultipleChoiceQuestions.DifficultLevel.Easy;
            if(model.difficultLevel == "Easy")
            {
                level = MultipleChoiceQuestions.DifficultLevel.Easy;
            }
            if(model.difficultLevel == "Normal") { level = MultipleChoiceQuestions.DifficultLevel.Normal; }
            if(model.difficultLevel == "Hard") { level = MultipleChoiceQuestions.DifficultLevel.Hard; }

            var newQuestion = new MultipleChoiceQuestions
            {
                difficultLevel = level,
                courseName = model.courseName,
                teacherEmail = result,
                questionName = model.questionName,
                answerA = model.answerA,
                answerB = model.answerB,
                answerC = model.answerC,
                answerD = model.answerD,
                correctAnswer = model.correctAnswer,
                create_At = DateTime.Now,
                update_At = DateTime.Now,
            };

            var question = _mapper.Map<MultipleChoiceQuestions>(newQuestion);
            _contex.Questions.Add(question);
            await _contex.SaveChangesAsync();   
            return ("create successfully .");
        }

        public async Task DeleteQuestionAsync(int id)
        {
            var delete = await _contex.Questions!.FindAsync(id);
            if (delete != null)
            {
                _contex.Questions.Remove(delete);
                await _contex.SaveChangesAsync();
            }
        }

        public async Task<List<QuestionsModel>> Search(string? search)
        {
            var question = _contex.Questions.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                question = _contex.Questions.Where(u => u.questionName.Contains(search));
            }
            var result = question.Select(u => new QuestionsModel
            {
                id = u.id,
                difficultLevel = u.difficultLevel.ToString(),
                courseName = u.courseName,
                teacherEmail = u.teacherEmail,
                questionName = u.questionName,
                answerA =   u.answerA,
                answerB= u.answerB,
                answerC = u.answerC,
                answerD = u.answerD,
                correctAnswer= u.correctAnswer,
            });
            return result.ToList();
        }

        public async Task<List<MultipleChoiceQuestions>> GetAll()
        {
            var questions = await _contex.Questions!.ToListAsync();
            return _mapper.Map<List<MultipleChoiceQuestions>>(questions);
        }

        public async Task<QuestionsModel> GetById(int id)
        {
            var question = await _contex.Questions!.FindAsync(id);
            return _mapper.Map<QuestionsModel>(question);
        }


        public async Task UpdateQuestionAsync(int id, QuestionsModel model)
        {
            if (id == model.id)
            {
                var level = MultipleChoiceQuestions.DifficultLevel.Easy;
                if (model.difficultLevel == "Easy")
                {
                    level = MultipleChoiceQuestions.DifficultLevel.Easy;
                }
                if (model.difficultLevel == "Normal") { level = MultipleChoiceQuestions.DifficultLevel.Normal; }
                if (model.difficultLevel == "Hard") { level = MultipleChoiceQuestions.DifficultLevel.Hard; }


                var question = await _contex.Questions!.FindAsync(model.id);
                question.difficultLevel = level;
                question.courseName = model.courseName;
                question.teacherEmail = question.teacherEmail;
                question.questionName = model.questionName;
                question.answerA = model.answerA;
                question.answerB = model.answerB;
                question.answerC = model.answerC;   
                question.answerD = model.answerD;
                question.courseName =model.courseName;
                question.create_At = question.create_At;
                question.update_At = DateTime.Now;
    
                _contex.Questions?.Update(question);
                await _contex.SaveChangesAsync();
            }
        }

        public  async Task AddExamByExistQuestion(string courseName, string examName,string time, int examNumber, int pointRange, int questionQuantity, int easyQuestionQuantity, int nomalQuestionQuantity, int hardQuestionQuantity)
        {
            var result = _httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.Email);
            var questions = _contex.Questions.AsQueryable();
            if (easyQuestionQuantity!= null && nomalQuestionQuantity!=null && hardQuestionQuantity != null )
            {
                questions = _contex.Questions
                    .Where( q => q.difficultLevel == MultipleChoiceQuestions.DifficultLevel.Easy && q.courseName == courseName).Take(easyQuestionQuantity)
                    .Union(_contex.Questions.Where(q => q.difficultLevel == MultipleChoiceQuestions.DifficultLevel.Normal && q.courseName == courseName).Take(nomalQuestionQuantity))
                    .Union(_contex.Questions.Where(q => q.difficultLevel == MultipleChoiceQuestions.DifficultLevel.Hard && q.courseName == courseName).Take(hardQuestionQuantity))
                    .OrderBy(r => EF.Functions.Random()).Take(100);
            }
            var target = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\System\Exam");
            if (!Directory.Exists(target))
            {
                Directory.CreateDirectory(target);
            }
            
            using(var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add(examName);

                worksheet.Cell(1, 1).Value = $"Course Name : {courseName}";
                worksheet.Cell(1, 2).Value = $"Exam Name : {examName}";
                worksheet.Cell(1,3).Value =  $"Point Range : {pointRange}";

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
                foreach (var question in questions)
                {
                    worksheet.Cell(row,1).Value = question.questionName;
                    worksheet.Cell(row, 2).Value = question.answerA;
                    worksheet.Cell(row, 3).Value = question.answerB;
                    worksheet.Cell(row, 4).Value = question.answerC;
                    worksheet.Cell(row, 5).Value = question.answerD;
                    worksheet.Cell(row, 6).Value = question.correctAnswer;
                    row++;
                }
                //Convert ExcelPackage to Byte array
                var filePath = Path.Combine(target, $"{examName}.xlsx");
                var filestream = new FileStream(filePath, FileMode.Create);
                workbook.SaveAs(filestream);
                filestream.Close();
                var exam = new Exam
                {
                    fileType = Path.GetExtension(filePath),
                    fileName = $"{examName}.xlsx",
                    filePath = filePath,
                    courseName = courseName,
                    teacherEmail = result,
                    examType = Exam.ExamType.Selected,
                    time = time,
                    examStatus = Exam.ExamStatus.Pendding,
                    create_At = DateTime.Now,

                };
                _contex.Exams?.Add(exam);
                await _contex.SaveChangesAsync();
            }
            

        }

        public async Task<List<QuestionsModel>> Filter(string? course, string? teacher, string? difficult)
        {
            var question = _contex.Questions.AsQueryable();
            if (!string.IsNullOrEmpty(course))
            {
                question = _contex.Questions.Where(m => m.courseName.Contains(course));
            }
            if (!string.IsNullOrEmpty(teacher))
            {
                question = _contex.Questions.Where(m => m.teacherEmail.Contains(teacher));
            }
            if (!string.IsNullOrEmpty(difficult))
            {
                var level = MultipleChoiceQuestions.DifficultLevel.Easy;
                if(difficult == "Easy") { level = MultipleChoiceQuestions.DifficultLevel.Easy; }
                if (difficult == "Normal") { level = MultipleChoiceQuestions.DifficultLevel.Normal; }
                if (difficult == "Hard") { level = MultipleChoiceQuestions.DifficultLevel.Hard; }
                question = _contex.Questions.Where(m => m.difficultLevel.Equals(level));
            }



            var result = question.Select(u => new QuestionsModel
            {
                id = u.id,
                difficultLevel = u.difficultLevel.ToString(),
                courseName = u.courseName,
                teacherEmail = u.teacherEmail,
                questionName = u.questionName,
                answerA = u.answerA,
                answerB = u.answerB,
                answerC = u.answerC,
                answerD = u.answerD,
                correctAnswer = u.correctAnswer,
            });
            return result.ToList();
        }

    
    }
}
    