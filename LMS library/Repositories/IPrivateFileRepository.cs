namespace LMS_library.Repositories
{
    public interface IPrivateFileRepository
    {
        public Task<List<PrivateFiles>> GetAll();
        public Task UpdateFileAsync(string newName, int id);
        public Task DeleteFileAsync(int id);
        public Task PostMultiFileAsync(List<IFormFile> privateFileUploads);
        public Task<List<PrivateFileModel>> Filter(string? type);
        public Task<List<PrivateFileModel>> Search(string? search);
    }
}
