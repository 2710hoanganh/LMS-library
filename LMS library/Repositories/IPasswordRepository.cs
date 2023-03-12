namespace LMS_library.Repositories
{
    public interface IPasswordRepository
    {
        public Task ChangePassword(int id, Password model);
    }
}
