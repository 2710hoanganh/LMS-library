namespace LMS_library.Repositories
{
    public interface IMaterialTopicRepository
    {
        public Task<List<Topic>> GetAll(int id);
        public Task<TopicModel> GetById(int id);
        public Task<string> AddTopicAsync(TopicModel model);
        public Task UpdateTopicAsync(int id, TopicModel model);
        public Task DeleteTopicAsync(int id);
    }
}
