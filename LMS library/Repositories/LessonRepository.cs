using AutoMapper;
using LMS_library.Data;

namespace LMS_library.Repositories
{
    public class LessonRepository : ILessonRepository
    {
        private readonly IMapper _mapper;
        private readonly DataDBContex _contex;

        public LessonRepository(IMapper mapper, DataDBContex contex)
        {
            _mapper = mapper;
            _contex = contex;
        }

        public async Task<string> AddLessonAsync(LessonModel model)
        {
            var topic = await _contex.Topics.FirstOrDefaultAsync(t => t.name == model.topicName);
            var material = await _contex.Materials.FirstOrDefaultAsync(m => m.name == model.materialName);
            if(topic == null ||material ==null) 
            {
                return ("Cant find topic or material");
            }
            if (material.fileStatus == CourseMaterial.FileStatus.Pendding || material.fileStatus == CourseMaterial.FileStatus.Reject)
            {
                return ("Material file not approved");
            } 
            var lesson = new Lesson
            {
                name = model.name,
                description= model.description,
                topicId = topic.id,
                materialId= material.id
            };

            var newLesson = _mapper.Map<Lesson>(lesson);
            _contex.Lessons.Add(newLesson);
            await _contex.SaveChangesAsync();
            return ("create successfully .");
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
                lesson.description = model.description;
                lesson.topicId =topic.id;
                lesson.materialId =material.id;
                var update = _mapper.Map<Lesson>(lesson);
                _contex.Lessons?.Update(update);
                await _contex.SaveChangesAsync();
            }
        }
    }
}
