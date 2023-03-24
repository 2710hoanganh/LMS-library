namespace LMS_library.Repositories
{
    public interface IMaterialTopicRepository
    {
        public Task<List<MaterialTopic>> GetAll();
        public Task<MaterialTopicModel> GetById(int id);
        public Task<string> AddTopicAsync(MaterialTopicModel model);
        public Task UpdateTopicAsync(int id, MaterialTopicModel model);
        public Task DeleteTopicAsync(int id);
    }
}
