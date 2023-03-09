using AutoMapper;
using LMS_library.Data;

namespace LMS_library.Repositories
{
    public class RoleRepository : IRoleRepository
    {

        private readonly IMapper _mapper;
        private readonly DataDBContex _contex;

        public RoleRepository(IMapper mapper ,DataDBContex  contex) 
        {
            _mapper = mapper;
            _contex = contex;
        }

        public async Task<string> AddRoleAsync(RoleModel model)
        {
            var newRole = _mapper.Map<Role>(model);

            _contex.Roles.Add(newRole);
            await _contex.SaveChangesAsync();
            return ("create successfully .");
        }

        public async Task DeleteRoleAsync(int id)
        {
            var deleteRole = await _contex.Roles!.FindAsync(id);
            if (deleteRole != null)
            {
                _contex.Roles.Remove(deleteRole);
                await _contex.SaveChangesAsync();
            }
        }

        public async Task<List<Role>> GetAll()
        {
            var role = await _contex.Roles!.ToListAsync();
            return _mapper.Map<List<Role>>(role);
        }

        public async Task<RoleModel> GetById(int id)
        {
            var role = await _contex.Roles!.FindAsync(id);
            return _mapper.Map<RoleModel>(role);
        }

        public async Task UpdateRoleAsync(int id, RoleModel model)
        {
            if (id == model.id)
            {
                var updateRole = _mapper.Map<Role>(model);
                _contex.Roles?.Update(updateRole);
                await _contex.SaveChangesAsync();
            }
        }
    }
}
