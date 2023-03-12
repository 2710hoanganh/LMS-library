using AutoMapper;
using Azure.Core;
using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;
using System.Text;

namespace LMS_library.Repositories
{
    public class UserEditRepository :IUserEditRepository
    {
        private readonly IMapper _mapper;
        private readonly DataDBContex _contex;

        public UserEditRepository(IMapper mapper, DataDBContex contex)
        {
            _mapper = mapper;
            _contex = contex;
        }


        public async Task UpdateUserAsync(int id, UserEditModel model)
        {
            if (id == model.id)
            {
                var user = await _contex.Users!.FindAsync(model.id);
                user.email = model.email;
                user.firstName = model.firstName;
                user.lastName = model.lastName;
                user.passwordHash = user.passwordHash;
                user.passwordSalt = user.passwordSalt;
                user.role = model.role;
                var updateUser = _mapper.Map<User>(user);
                _contex.Users?.Update(updateUser);
                await _contex.SaveChangesAsync();
            }
        }





 
    }
}
