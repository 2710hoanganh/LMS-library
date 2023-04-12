using AutoMapper;
using LMS_library.Data;
using System.Security.Claims;

namespace LMS_library.Repositories
{
    public class ResourceRepository : IResourceRepository
    {

        private readonly IMapper _mapper;
        private readonly DataDBContex _contex;
        private readonly HttpContextAccessor _httpContextAccessor;


        public ResourceRepository(IMapper mapper, DataDBContex contex, HttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;
            _contex = contex;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<string> AddResourceAsync(ResourceModel model)
        {
            var course = await _contex.Courses!.FirstOrDefaultAsync(c => c.courseName == model.courseName);
            var topic = await _contex.Topics!.FirstOrDefaultAsync(t => t.courses.courseName == course.courseName&& t.name == model.topicName);
            var lesson = await _contex.Lessons.FirstOrDefaultAsync( l => l.topic.id == topic.id);
            var material = await _contex.Materials.FirstOrDefaultAsync(m => m.name == model.materialName && m.ResourceList ==null && m.MaterialType.name.Equals("Resource"));
            if ( lesson == null || material == null ) { return ("Not found"); }
            if(material.MaterialType.name != "Resource") { return ("Material type must be resource !"); }
            var resource = new ResourceList
            {
                lessonId = lesson.id,
                materialId = material.id
            };

            var newResource = _mapper.Map<ResourceList>(resource);
            _contex.ResourceLists.Add(newResource);
            await _contex.SaveChangesAsync();
            return ("create successfully .");
        }

        public async Task<string> AddResourceAndUploadFileAsync(string courseName, string topicName, string lessonName, IFormFile formFile)
        {
            var course = await _contex.Courses!
                .FirstOrDefaultAsync(c => c.courseName == courseName);
            var topic = await _contex.Topics!
                .FirstOrDefaultAsync(t => t.courses.courseName == courseName && t.name == topicName);
            var lesson = await _contex.Lessons!
                .FirstOrDefaultAsync(l => l.name == lessonName && l.topicId == topic.id);
            var fileType = await _contex.MaterialTypes!
                .FirstOrDefaultAsync(t => t.name == "Resource");
            var target = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\System\Course Material");
            if (course == null || topic == null || fileType == null) { return (""); }
            if (!Directory.Exists(target))
            {
                Directory.CreateDirectory(target);
            }
            if (formFile == null) { return ("PLease Choose File To Upload!"); }

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

            var newResource = new ResourceList
            {
                lessonId = lesson.id,
                materialId = newfile.id,
            };
            var createNEw = _mapper.Map<ResourceList>(newResource);
            _contex.ResourceLists.Add(newResource);
            await _contex.SaveChangesAsync();
            return ("Create lesson and upload file for leader review successfully !");
        }


        public async Task AddToTopic(string topicName, int id)
        {
            throw new NotImplementedException();
        }

        public async Task Delete(int id)
        {
            var delete = await _contex.ResourceLists!.FindAsync(id);
            if (delete != null)
            {

                _contex.ResourceLists.Remove(delete);
                await _contex.SaveChangesAsync();
            }
        }

        public async Task<List<ResourceList>> GetAll(int id)
        {
            var resources = await _contex.ResourceLists!.ToListAsync();
            return _mapper.Map<List<ResourceList>>(resources);
        }

        public async Task<List<ResourceList>> GetAllOnLesson(int id)
        {
            var resource = await _contex.ResourceLists!.Where(l => l.lessonId == id).ToListAsync();
            return _mapper.Map<List<ResourceList>>(resource);
        }

        public async Task Update(int id, ResourceModel model)
        {
            var lesson = await _contex.Lessons.FirstOrDefaultAsync(l => l.name == model.lessonName);
            var resource = await _contex.ResourceLists.FindAsync(id);
            if (resource != null|| lesson != null)
            {
                resource.lessonId = lesson.id;
                var update = _mapper.Map<ResourceList>(resource);
                _contex.ResourceLists?.Update(update);
                await _contex.SaveChangesAsync();
            }
        }
        public async Task AddToResource(string lessonName, int id)
        {
            var material = await _contex.Materials!.FindAsync(id);
            var lesson = await _contex.Lessons!.FirstOrDefaultAsync(t => t.name == lessonName);
            var resource = await _contex.ResourceLists.FirstOrDefaultAsync(r => r.lessonId == lesson.id);
            if (lesson == null || material == null || resource == null || material.fileStatus == CourseMaterial.FileStatus.Reject || material.fileStatus == CourseMaterial.FileStatus.Pendding)
            {
                return;
            }

            material.name = material.name;
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


        private string UserInfo()
        {
            var result = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            return result;
        }
    }
}
