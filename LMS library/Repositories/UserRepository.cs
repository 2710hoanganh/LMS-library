using AutoMapper;
using System.Security.Cryptography;

namespace LMS_library.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IMapper _mapper;
        private readonly DataDBContex _contex;

        public UserRepository(DataDBContex contex, IMapper mapper)
        {
            _contex = contex;
            _mapper = mapper;
        }

        public async Task<string> AddUserAsync(UserModel model)
        {
            var roleId = await _contex.Roles.FirstOrDefaultAsync(r => r.name == model.role);
            if (roleId == null)
            {
                return ("Role Not Existing");
            }
            HashPassword(model.password
              , out byte[] passswordHash
              , out byte[] passwordSalt);
            var user = new User
            {
                userCode = model.userCode,
                email = model.email,
                firstName = model.firstName,
                lastName = model.lastName,
                passwordHash = Convert.ToHexString(passswordHash),
                passwordSalt = Convert.ToHexString(passwordSalt),
                roleId = roleId.id,

            };
            var newUser = _mapper.Map<User>(user);

            _contex.Users.Add(newUser);
            await _contex.SaveChangesAsync();
            return ($"{newUser.email} create successfully .");

        }

        public async Task DeleteUserAsync(int id)
        {
            var deleteUser = await _contex.Users!.FindAsync(id);
            if (deleteUser != null)
            {
                _contex.Users.Remove(deleteUser);
                await _contex.SaveChangesAsync();
            }
        }

        public async Task<List<UserModel>> Filter(string? filter)
        {
            var user = _contex.Users.AsQueryable();

            if (!string.IsNullOrEmpty(filter))
            {
                user = _contex.Users.Where(u => u.Role.name.Contains(filter));
            }
            var result = user.Select(u => new UserModel
            {
                id = u.id,
                userCode = u.userCode,
                email = u.email,
                firstName = u.firstName,
                lastName = u.lastName,
                role = u.Role.name
            });
            return result.ToList();
        }

        public async Task<List<User>> GetAll()
        {
            var user = await _contex.Users!.ToListAsync();
            return _mapper.Map<List<User>>(user);

        }
        public async Task<User> GetById(int id)
        {
            var user = await _contex.Users!.FindAsync(id);
            return _mapper.Map<User>(user);
        }

        public async Task<List<UserModel>> Search(string? search)
        {
            var user = _contex.Users.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                user =_contex.Users.Where(u => u.email.Contains(search)|| u.userCode.Contains(search) || u.firstName.Contains(search)|| u.lastName.Contains(search));
            }
            var result = user.Select(u => new UserModel
            {
                id = u.id,
                userCode= u.userCode,
                email = u.email,
                firstName = u.firstName,    
                lastName = u.lastName,
                role = u.Role.name
            });
            return result.ToList(); 
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
