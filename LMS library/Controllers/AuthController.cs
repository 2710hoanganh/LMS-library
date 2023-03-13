using LMS_library.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace LMS_library.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
       
        private readonly ISentMailRepository _mailRepository;
        private readonly DataDBContex _contex;
        private  IConfiguration _configuration;

   
        public AuthController(DataDBContex contex , IConfiguration configuration ,ISentMailRepository mailRepository)
        {
            _contex = contex;
            _configuration = configuration;
            _mailRepository = mailRepository;
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

        [HttpPost("Forgot-Password")]
        public async Task<IActionResult> ForgotPassword(string email)
        {
           var user = await _contex.Users.FirstOrDefaultAsync(u => u.email == email);
           if (user == null)
           {
                return BadRequest("User not existing");
           }
            user.resetToken = CreateRamdomToken();
            user.resetTokenExpires = DateTime.Now.AddHours(1);
            await _contex.SaveChangesAsync();

            var model = new SentMail
            {
                To = user.email,
                Subject = $"Reset Password Token {user.email}",
                Body = $"Here is your token {user.resetToken} , token expires in 5 minutes",
            };
            _mailRepository.SendEmail(model);


            return Ok("The reset token will sent to your email !");
        }

        [HttpPost("Reset-Password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequest model)
        {
            var user = await _contex.Users.FirstOrDefaultAsync(u => u.resetToken == model.token);
            if (user == null || user.resetTokenExpires < DateTime.Now )
            {
                return BadRequest("Ivalid Token");
            }

            HashPassword(model.password
                , out byte[] passswordHash
                , out byte[] passwordSalt);

            user.passwordHash = Convert.ToHexString(passswordHash);
            user.passwordSalt = Convert.ToHexString(passwordSalt);
            user.resetToken = null;
            user.resetTokenExpires = null;

            await _contex.SaveChangesAsync();

            return Ok("Password Change Successfully !");
        }






        //function create token use to rs password
        private string CreateRamdomToken()
        {
            return Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
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
