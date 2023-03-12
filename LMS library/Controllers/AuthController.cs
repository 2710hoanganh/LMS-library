using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace LMS_library.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
       

        private readonly DataDBContex _contex;
        private  IConfiguration _configuration;

   
        public AuthController(DataDBContex contex , IConfiguration configuration)
        {
            _contex = contex;
            _configuration = configuration;
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterRequest request)
        {
            if(_contex.Users.Any(u => u.email == request.email))
            {
                return BadRequest("User already exists .");
            }

            //hash password
            HashPassword(request.password
                ,out byte[] passswordHash 
                ,out byte[] passwordSalt);

            var user = new User
            {
                email = request.email,
                passwordHash = Convert.ToHexString(passswordHash),
                passwordSalt = Convert.ToHexString(passwordSalt),
                role = request.role,

            };
             _contex.Users.Add(user);
            await _contex.SaveChangesAsync();
            return Ok(new
            {
                user.passwordHash,
                user.passwordSalt
            });

        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginRequest request)
        {
           var user = await _contex.Users.FirstOrDefaultAsync(u => u.email == request.email);
            if (user == null)
            {
                return BadRequest("User not existing");
            }
            if (!VerifyHashPassword(request.password, Convert.FromHexString(user.passwordHash), Convert.FromHexString(user.passwordSalt)))
            {
                return BadRequest("Password not correct");
            }
            string token = CreateToken(user);


            return Ok(token) ;



        }

        [Authorize]
        [HttpPost("Logout")]
        public ActionResult Logout()
        {
            

            return Ok();
        }

        //function create token use in Login
        private string CreateToken(User user) 
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.email),
                new Claim(ClaimTypes.Role, user.role),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat,DateTime.UtcNow.ToString()),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JsonWebTokenKeys:Key"]));

            var cred = new SigningCredentials(key , SecurityAlgorithms.HmacSha256 );

            var token = new JwtSecurityToken(
                issuer: _configuration["JsonWebTokenKeys:Issuer"],
                audience: _configuration["JsonWebTokenKeys:Audience"],
                claims:claims,
                expires:DateTime.Now.AddDays(1),
                signingCredentials:cred);
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);  

            return jwt;
        }


        

        //Hash password function 
        private void HashPassword(string password , out byte[] passswordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passswordHash = hmac
                    .ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }



        //verify hash password when login 
        private bool VerifyHashPassword(string password, byte[] passswordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac
                    .ComputeHash(Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passswordHash);
            }
        }
    }
}
