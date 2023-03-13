namespace LMS_library.Repositories
{
    public interface ISentMailRepository
    {

        void SendEmail(SentMail request);
    }
}
