using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace LMS_library.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IMapper _mapper;
        private readonly DataDBContex _contex;

        public UserRepository(DataDBContex contex ,IMapper mapper) 
        {
            _contex = contex;
            _mapper = mapper;
        }

        public async Task<int> AddUserAsync(UserModel model)
        {
            var newUser = _mapper.Map<User>(model); 
            _contex.Users.Add(newUser);
            await _contex.SaveChangesAsync();
            return newUser.id;

        }

        public async Task DeleteUserAsync(int id)
        {
            var deleteUser = await _contex.Users!.FindAsync(id);
            if(deleteUser != null)
            {
                _contex.Users.Remove(deleteUser);
                await _contex.SaveChangesAsync();   
            }
        }

        public async Task<List<UserModel>> GetAll()
        {
            var user = await _contex.Users!.ToListAsync();
            return  _mapper.Map<List<UserModel>>(user);
            
            
        }

        public async Task<UserModel> GetById(int id)
        {
            var user = await _contex.Users!.FindAsync(id);
            return _mapper.Map<UserModel>(user);
        }

        async Task IUserRepository.UpdateUserAsync(int id, UserModel model)
        {
            if(id == model.id)
            {
                var updateUser = _mapper.Map<User>(model);
                _contex.Users?.Update(updateUser);
                await _contex.SaveChangesAsync();
            }
        }
    }
}
