using LMS_library.Migrations;

namespace LMS_library.Repositories
{
    public interface ICourseMaterialRepository
    {
        public Task<List<CourseMaterial>> GetAllForStudent(int id);
        public Task<List<CourseMaterial>> GetAll(int id);
        public Task FileApprove(string check , int id);

        public Task UpdateFileAsync(string newName, int id);
        public Task AddToTopic(string topicName, int id);
        public Task DeleteFileAsync(int id);
        public Task PostMultiFileAsync( string type, string course ,List<IFormFile> privateFileUploads);
    }
}
