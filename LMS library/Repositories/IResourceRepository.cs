namespace LMS_library.Repositories
{
    public interface IResourceRepository
    {
        public Task<List<ResourceList>> GetAll(int id);
        public Task<List<ResourceList>> GetAllOnLesson(int id);
        public Task<string> AddResourceAsync(ResourceModel model);
        public Task<string> AddResourceAndUploadFileAsync(string courseName , string topicName ,string lessonName ,IFormFile formFile);
        public Task Update(int id, ResourceModel model);

        public Task Delete(int id);
        public Task AddToTopic(string topicName, int id);
        public Task AddToResource(string lessonName, int id);
    }
}
