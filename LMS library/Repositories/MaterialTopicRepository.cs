using AutoMapper;
using LMS_library.Data;

namespace LMS_library.Repositories
{
    public class MaterialTopicRepository : IMaterialTopicRepository
    {


        private readonly IMapper _mapper;
        private readonly DataDBContex _contex;

        public MaterialTopicRepository(IMapper mapper, DataDBContex contex)
        {
            _mapper = mapper;
            _contex = contex;
        }
        public async Task<string> AddTopicAsync(TopicModel model)
        {
            var course = await _contex.Courses.FirstOrDefaultAsync(u => u.courseName == model.courseName);
            if (course == null) { return ("Course not existing"); }
            var topic = new Topic
            {
                name = model.name,
                description= model.description,
                courseId = course.id
            };
            var newTopic = _mapper.Map<Topic>(topic);
            _contex.Topics.Add(newTopic);
            await _contex.SaveChangesAsync();
            return ("create successfully .");
        }

        public async Task DeleteTopicAsync(int id)
        {
            var delete = await _contex.Topics!.FindAsync(id);
            _contex.Topics.Remove(delete);
            await _contex.SaveChangesAsync();
        }

        public async Task<List<Topic>> GetAll(int id)
        {
            var topic = await _contex.Topics!.Where(t => t.courseId == id).ToListAsync();
            return _mapper.Map<List<Topic>>(topic);
        }

        public async Task<TopicModel> GetById(int id)
        {
            var topic = await _contex.Topics!.FindAsync(id);
            return _mapper.Map<TopicModel>(topic);
        }

        public async Task UpdateTopicAsync(int id, TopicModel model)
        {
            var course = await _contex.Courses.FirstOrDefaultAsync(u => u.courseName == model.courseName);
            var topic = await _contex.Topics!.FindAsync(model.id);
            topic.name = model.name;
            topic.description = model.description;  
            course.id = course.id;
            var update = _mapper.Map<Topic>(topic);
            _contex.Topics?.Update(update);
            await _contex.SaveChangesAsync();
        }
    }
}
