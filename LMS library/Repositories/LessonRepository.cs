using AutoMapper;
using DocumentFormat.OpenXml.Spreadsheet;
using LMS_library.Data;
using System.Collections.Immutable;
using System.Security.Claims;

namespace LMS_library.Repositories
{
    public class LessonRepository : ILessonRepository
    {
        private readonly IMapper _mapper;
        private readonly DataDBContex _contex;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LessonRepository(IHttpContextAccessor httpContextAccessor,IMapper mapper, DataDBContex contex)
        {
            _mapper = mapper;
            _contex = contex;
            _httpContextAccessor= httpContextAccessor;
        }


        public async Task<string> AddLessonAsync(LessonModel model)
        {
            var course = await _contex.Courses!
                .FirstOrDefaultAsync(c =>c.courseName == model.courseName);
            var topic = await _contex.Topics
                .FirstOrDefaultAsync(t => t.name == model.topicName && t.courses.courseName == model.courseName);
            var material = await _contex.Materials
                .FirstOrDefaultAsync(m => m.name == model.materialName && m.MaterialType.name.Equals("Lesson")&& m.Lesson ==null);
            var type = await _contex.MaterialTypes!.FirstOrDefaultAsync(t => t.id == material!.materialTypeID);
            if(topic == null ||material ==null) 
            {
                return ("Cant find topic or material");
            }
            if (material.fileStatus == CourseMaterial.FileStatus.Pendding || material.fileStatus == CourseMaterial.FileStatus.Reject)
            {
                return ("Material file not approved");
            } 
            if(type.name != "Lesson")
            {
                return ("Material type must be lesson !");
            }
            var lesson = new Lesson
            {
                name = model.name,
                topicId = topic.id,
                materialId= material.id
            };

            var newLesson = _mapper.Map<Lesson>(lesson);
            _contex.Lessons.Add(newLesson);
            await _contex.SaveChangesAsync();
            return ("create successfully .");
        }
        public async Task<string> AddLessonAndUploadFileAsync(string courseName, string topicName, string lessonTitle,IFormFile formFile)
        {
            var course = await _contex.Courses!
                .FirstOrDefaultAsync(c =>c.courseName == courseName);
            var topic = await _contex.Topics!
                .FirstOrDefaultAsync(t => t.courses.courseName == courseName&& t.name ==topicName);

            var fileType = await _contex.MaterialTypes!
                .FirstOrDefaultAsync(t => t.name =="Lesson");
            var target = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\System\Course Material");
            if(course == null || topic == null || fileType == null) { return (""); }
            if (!Directory.Exists(target))
            {
                Directory.CreateDirectory(target);
            }
            if(formFile== null) { return ("PLease Choose File To Upload!"); }

            var filePath = Path.Combine(target, formFile.FileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await formFile.CopyToAsync(stream);
            }
            var newfile = new CourseMaterial
            {
                name = formFile.FileName,
                materialTypeID = fileType.id,
                UserId = Int32.Parse(UserInfo()),
                courseId = course.id,
                fileStatus = 0,
                materialPath = filePath,
                fileSize = filePath.Length,
                submission_date = DateTime.Now,
            };
            _contex.Materials?.Add(newfile);
            await _contex.SaveChangesAsync();

            var newLesson = new Lesson
            {
                name = lessonTitle,
                topicId=topic.id,
                materialId= newfile.id,
            };
            var createNEw = _mapper.Map<Lesson>(newLesson);
            _contex.Lessons.Add(newLesson);
            await _contex.SaveChangesAsync();
            return ("Create lesson and upload file for leader review successfully !");
        }

        public async Task DeleteLessonAsync(int id)
        {
            var lesson = await _contex.Lessons!.FindAsync(id);
            var material = await _contex.Materials.FirstAsync(m => m.id == lesson.materialId);
            if (lesson != null)
            {
                _contex.Lessons.Remove(lesson);
                await _contex.SaveChangesAsync();
            }
        }

        public async Task<List<Lesson>> GetAll()
        {
            var lesson = await _contex.Lessons!.ToListAsync();
            return _mapper.Map<List<Lesson>>(lesson);
        }
        public async Task<List<Lesson>> GetAllOnTopic(int id)//topic id
        {
            var lesson = await _contex.Lessons!.Where(l => l.topicId ==id).ToListAsync();
            return _mapper.Map<List<Lesson>>(lesson);
        }

        public async Task<LessonModel> GetById(int id)
        {
            var lesson = await _contex.Lessons!.FindAsync(id);
            return _mapper.Map<LessonModel>(lesson);
        }

        public async Task UpdateLessonAsync(int id, LessonModel model)
        {
            if(id == model.id)
            {
                var topic = await _contex.Topics.FirstOrDefaultAsync(t => t.name == model.topicName);
                var material = await _contex.Materials.FirstOrDefaultAsync(m => m.name == model.materialName);
                if (topic == null || material == null)
                {
                    return ;
                }
                if (material.fileStatus == CourseMaterial.FileStatus.Pendding || material.fileStatus == CourseMaterial.FileStatus.Reject)
                {
                    return ;
                }
                var lesson = await _contex.Lessons!.FindAsync(model.id);
                lesson.name = model.name;
                lesson.topicId =topic.id;
                lesson.materialId =material.id;
                var update = _mapper.Map<Lesson>(lesson);
                _contex.Lessons?.Update(update);
                await _contex.SaveChangesAsync();
            }
        }



        private string UserInfo()
        {
            var result = _httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier);
            return result;
        }
    }
}
