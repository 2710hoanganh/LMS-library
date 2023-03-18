

namespace LMS_library.Repositories
{
    public interface IPrivateFileRepository
    {
        public Task<List<PrivateFiles>> GetAll();
        public Task UpdateFileAsync(int id, PrivateFileModel model);
        public Task DeleteFileAsync(int id);
        public Task PostMultiFileAsync(List<IFormFile> privateFileUploads);
    }
}
