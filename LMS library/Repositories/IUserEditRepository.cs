namespace LMS_library.Repositories
{
    public interface IUserEditRepository
    {
        public Task UpdateUserAsync(int id, UserEditModel model);
        public Task AddStudentToClass(int id ,string student);//student Email
    }
}
