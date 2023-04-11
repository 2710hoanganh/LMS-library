using AutoMapper;
using Org.BouncyCastle.Asn1.IsisMtt.X509;

namespace LMS_library.Repositories
{
    public class UserEditRepository : IUserEditRepository
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
                var roleId = await _contex.Roles.FirstOrDefaultAsync(r => r.name == model.role);
                var user = await _contex.Users!.FindAsync(model.id);
                var sex = User.Sex.None;
                if (model.sex == "Male") { sex = User.Sex.Male; }
                if (model.sex == "Female") { sex = User.Sex.Female; }
                if (user != null || roleId != null)
                {
                    user.userCode = model.userCode;
                    user.email = model.email;
                    user.firstName = model.firstName;
                    user.lastName = model.lastName;
                    user.image = user.image;
                    user.sex = sex;
                    user.phone = model.phone;
                    user.address = model.address;
                    user.passwordHash = user.passwordHash;
                    user.passwordSalt = user.passwordSalt;
                    user.roleId = roleId.id;
                    var updateUser = _mapper.Map<User>(user);
                    _contex.Users?.Update(updateUser);
                    await _contex.SaveChangesAsync();
                }
            }
        }
    }
}
