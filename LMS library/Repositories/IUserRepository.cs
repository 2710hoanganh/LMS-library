using LMS_library.Data;
using LMS_library.Models;
namespace LMS_library.Repositories
{
    public interface IUserRepository
    {
        public Task<List<UserModel>> GetAll();
        public Task<UserModel> GetById(int id);
        public Task<int> AddUserAsync(UserModel model);
        public Task UpdateUserAsync(int id ,UserModel model);
        public Task DeleteUserAsync(int id);
    }
}
