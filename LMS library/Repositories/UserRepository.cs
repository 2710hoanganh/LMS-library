using AutoMapper;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Security.Cryptography;

namespace LMS_library.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IMapper _mapper;
        private IWebHostEnvironment _webHostEnvironment;
        private readonly DataDBContex _contex;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserRepository(DataDBContex contex, IMapper mapper, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment webHostEnvironment)
        {
            _contex = contex;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<string> AddUserAsync(UserModel model)
        {
            var roleId = await _contex.Roles.FirstOrDefaultAsync(r => r.name == model.role);
            var sex = User.Sex.None;
            if (roleId == null)
            {
                return ("Role Not Existing");
            }
            if(model.sex == "Male") { sex = User.Sex.Male; }
            if(model.sex == "Female") { sex = User.Sex.Female; }

            HashPassword(model.password
              , out byte[] passswordHash
              , out byte[] passwordSalt);



            var user = new User
            {
                userCode = model.userCode,
                email = model.email,
                firstName = model.firstName,
                lastName = model.lastName,
                image = null,
                sex = sex,
                phone = model.phone,
                address= model.address,
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
                System.IO.File.Delete(deleteUser.image);
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


        public async Task UploadImage(int id, IFormFile formFile)
        {
            var user = await _contex.Users!.FindAsync(id);
            if (user == null)
            {
                return;
            }
            if(formFile == null) { return; }
            var target = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\User\Image");
            if(!Directory.Exists(target))
            {
                Directory.CreateDirectory(target);
            }
            var filePath = Path.Combine(target, formFile.FileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await formFile.CopyToAsync(stream);
            }
            user.userCode = user.userCode;
            user.email = user.email;
            user.firstName = user.firstName;
            user.lastName = user.lastName;
            user.image = filePath;
            user.sex= user.sex;
            user.phone= user.phone;
            user.address = user.address;
            user.passwordHash = user.passwordHash;
            user.passwordSalt = user.passwordSalt;
            user.roleId = user.roleId;
            var updateUser = _mapper.Map<User>(user);
            _contex.Users?.Update(updateUser);
            await _contex.SaveChangesAsync();
        }
        public async Task ChangeImage(int id, IFormFile formFile)
        {
            var user = await _contex.Users!.FindAsync(id);
            if (user == null)
            {
                return;
            }
            if (formFile == null) { return; }
            var target = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\User\Image");
            if (!Directory.Exists(target))
            {
                Directory.CreateDirectory(target);
            }
            var filePath = Path.Combine(target, formFile.FileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await formFile.CopyToAsync(stream);
            }
            System.IO.File.Delete(user.image);
            user.userCode = user.userCode;
            user.email = user.email;
            user.firstName = user.firstName;
            user.lastName = user.lastName;
            user.image = filePath;
            user.sex = user.sex;
            user.phone = user.phone;
            user.address = user.address;
            user.passwordHash = user.passwordHash;
            user.passwordSalt = user.passwordSalt;
            user.roleId = user.roleId;
            var updateUser = _mapper.Map<User>(user);
            _contex.Users?.Update(updateUser);
            await _contex.SaveChangesAsync();
        }

        public async Task DeleteImageAsync(int id)
        {
            var user = await _contex.Users!.FindAsync(id);
            if (user == null)
            {
                return;
            }
            System.IO.File.Delete(user.image);
            user.userCode = user.userCode;
            user.email = user.email;
            user.firstName = user.firstName;
            user.lastName = user.lastName;
            user.image = null;
            user.sex = user.sex;
            user.phone = user.phone;
            user.address = user.address;
            user.passwordHash = user.passwordHash;
            user.passwordSalt = user.passwordSalt;
            user.roleId = user.roleId;
            var updateUser = _mapper.Map<User>(user);
            _contex.Users?.Update(updateUser);
            await _contex.SaveChangesAsync();
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
