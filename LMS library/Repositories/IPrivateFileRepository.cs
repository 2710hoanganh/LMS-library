using static LMS_library.Data.PrivateFiles;

namespace LMS_library.Repositories
{
    public interface IPrivateFileRepository
    {
        public Task PostFileAsync( PrivateFileUpload privateFileUpload);
        public Task DownloadFileById(int id);
    }
}
