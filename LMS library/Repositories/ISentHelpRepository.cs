namespace LMS_library.Repositories
{
    public interface ISentHelpRepository
    {
        public Task<string> SentHelp(SentHelpModel model);
    }
}
