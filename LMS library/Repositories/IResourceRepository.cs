namespace LMS_library.Repositories
{
    public interface IResourceRepository
    {
        public Task<List<ResourceList>> GetAll(int id);
        public Task<List<ResourceList>> GetAllOnLesson(int id);
        public Task<string> AddRessourceAsync(ResourceModel model);
        public Task Update(int id, ResourceModel model);

        public Task Delete(int id);
        public Task AddToTopic(string topicName, int id);
        public Task AddToResource(string lessonName, int id);
    }
}
