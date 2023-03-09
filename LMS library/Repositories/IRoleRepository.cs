using LMS_library.Data;
using LMS_library.Models;
namespace LMS_library.Repositories
{
    public interface IRoleRepository
    {
        public Task<List<Role>> GetAll();
        public Task<RoleModel> GetById(int id);
        public Task<string> AddRoleAsync(RoleModel model);
        public Task UpdateRoleAsync(int id, RoleModel model);
        public Task DeleteRoleAsync(int id);
    }
}
