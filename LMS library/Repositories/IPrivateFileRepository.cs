using static LMS_library.Data.PrivateFiles;

namespace LMS_library.Repositories
{
    public interface IPrivateFileRepository
    {
        public Task PostMultiFileAsync(List<IFormFile> privateFileUploads);
        public Task DownloadFileById(int id);
    }
}
