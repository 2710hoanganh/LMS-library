using AutoMapper;
using System.Security.Cryptography;
using System.Text;

namespace LMS_library.Repositories
{
    public class PasswordRepository :IPasswordRepository
    {
        private readonly IMapper _mapper;
        private readonly DataDBContex _contex;

        public PasswordRepository(IMapper mapper, DataDBContex contex)
        {
            _mapper = mapper;
            _contex = contex;
        }



        public async Task ChangePassword(int id, Password model)
        {
            if (id == model.id)
            {
                var user = await _contex.Users!.FindAsync(model.id);
                if ( VerifyHashPassword(model.oldPassword, Convert.FromHexString(user.passwordHash), Convert.FromHexString(user.passwordSalt)))
                {
                    HashPassword(model.password
                    , out byte[] passswordHash
                    , out byte[] passwordSalt);
                    user.userCode = user.userCode;
                    user.email = user.email;
                    user.firstName = user.firstName;
                    user.lastName = user.lastName;
                    user.passwordHash = Convert.ToHexString(passswordHash);
                    user.passwordSalt = Convert.ToHexString(passwordSalt);
                    user.roleId = user.roleId;
                    var updateUser = _mapper.Map<User>(user);
                    _contex.Users?.Update(updateUser);
                    await _contex.SaveChangesAsync();
                }
            }
        }


        //verify hash password 
        private bool VerifyHashPassword(string password, byte[] passswordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac
                    .ComputeHash(Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passswordHash);
            }
        }
        //Hash password function 
        private void HashPassword(string password, out byte[] passswordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passswordHash = hmac
                    .ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
    }
}
