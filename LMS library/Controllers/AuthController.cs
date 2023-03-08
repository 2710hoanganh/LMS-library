using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace LMS_library.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
       

        private DataDBContex _contex;
        private readonly IConfiguration _configuration;

   
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

            HashPassword(request.password
                ,out byte[] passswordHash 
                ,out byte[] passwordSalt);

            var user = new User
            {
                email = request.email,
                passwordHash = passswordHash,
                passwordSalt = passwordSalt

            };
             _contex.Users.Add(user);
            await _contex.SaveChangesAsync();
            return Ok( "User create successfully !");

        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginRequest request)
        {
           var user = await _contex.Users.FirstOrDefaultAsync(u => u.email == request.email);
            if (user == null)
            {
                return BadRequest("User not existing");
            }
            if (!VerifyHashPassword(request.password, user.passwordHash, user.passwordSalt))
            {
                return BadRequest("Password not correct");
            }
            string token = CreateToken(user);


            return Ok($"Wellcome back {user.email} , Token : {token}") ;



        }


        private string CreateToken(User user) 
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.email),
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration["JsonWebTokenKeys:IssuerSigningKey"]));

            var cred = new SigningCredentials(key , SecurityAlgorithms.HmacSha512Signature );

            var token = new JwtSecurityToken(
                claims:claims,
                expires:DateTime.Now.AddDays(1),
                signingCredentials:cred);
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);  

            return jwt;
        }
        
        private void HashPassword(string password , out byte[] passswordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passswordHash = hmac
                    .ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyHashPassword(string password, byte[] passswordHash,  byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac
                    .ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passswordHash);
            }
        }
    }
}
