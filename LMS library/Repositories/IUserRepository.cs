using LMS_library.Data;
using LMS_library.Models;
namespace LMS_library.Repositories
{
    public interface IUserRepository
    {
        public Task<IEnumerable<User>> GetAll();
        public Task<User> GetById(int id);
        public Task<string> AddUserAsync(UserModel model);
        public Task DeleteUserAsync(int id);
        public Task UploadImage(int id, IFormFile formFile); //user id
        public Task ChangeImage (int id, IFormFile formFile);
        public Task DeleteImageAsync(int id);

        public Task<List<UserModel>> Search(string? search);
        public Task<List<UserModel>> Filter(string? filter);
    }
}
