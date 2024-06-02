using AutoMapper;
using LMS_library.Data_Service;
using Microsoft.AspNetCore.Mvc;

namespace LMS_library.Repositories
{
    public class RoleRepository : IRoleRepository
    {

        private readonly IMapper _mapper;
        private readonly DataDBContex _contex;
        private readonly ICaching _caching;

        public RoleRepository(IMapper mapper, DataDBContex contex ,ICaching caching)
        {
            _mapper = mapper;
            _contex = contex;
            _caching = caching;
        }

        public async Task<string> AddRoleAsync(RoleModel model)
        {
            var newRole = _mapper.Map<Role>(model);
            newRole.create_At = DateTime.Now;
            _contex.Roles.Add(newRole);
            await _contex.SaveChangesAsync();
            return ("create successfully .");
        }

        public async Task DeleteRoleAsync(int id)//role id
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
            var cacheData = _caching.GetData<IEnumerable<Role>>("roles");
            if(cacheData !=null && cacheData.Count() > 0)
            {
                return cacheData.ToList();
            }
            else
            {
                cacheData = await _contex.Roles.ToListAsync();
                var expiryTime = DateTimeOffset.Now.AddSeconds(30);
                _caching.SetData<IEnumerable<Role>>("roles" , cacheData , expiryTime);
                return cacheData.ToList();
            }
        }

        public async Task<RoleModel> GetById(int id)//role id
        {
            var role = await _contex.Roles!.FindAsync(id);
            return _mapper.Map<RoleModel>(role);
        }

        public async Task<List<RoleModel>> Filter(string? filter)
        {
            var role = _contex.Roles.AsQueryable();

            if (!string.IsNullOrEmpty(filter))
            {
                role = _contex.Roles.Where(u => u.name.Contains(filter));
            }
            var result = role.Select(u => new RoleModel 
            {
                id = u.id,
                name = u.name,
                create_At = u.create_At,
                update_At= u.update_At,
            });
            return result.ToList();
        }


        public async Task<string> TestAsyn([FromForm] RoleModel model)
        {

            
                var newRole = _mapper.Map<Role>(model);
                newRole.create_At = DateTime.Now;
                _contex.Roles.Add(newRole);
                await _contex.SaveChangesAsync();

            return ("create successfully .");
        }

        public async Task UpdateRoleAsync(int id, RoleModel model)
        {
            if (id == model.id)
            {
                var role = await _contex.Roles!.FindAsync(model.id);
                role.name = model.name;
                role.description = model.description;
                role.create_At = role.create_At;
                role.update_At = model.update_At;
                var updateRole = _mapper.Map<Role>(role);
                _contex.Roles?.Update(updateRole);
                await _contex.SaveChangesAsync();
            }
        }
    }
}
