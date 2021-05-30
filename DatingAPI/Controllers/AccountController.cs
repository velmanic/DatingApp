using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.DatingAPI.Data;
using API.DatingAPI.Entities;
using DatingAPI.DTOs;
using DatingAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatingAPI.Controllers
{
    public class AccountController : BaseAPIController
    {
        private readonly DataContext context;
        private readonly ITokenService _tokenService;

        public AccountController(DataContext context, ITokenService tokenService)
        {
            this.context = context;
            this._tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if(await UserExists(registerDto.Username)) return BadRequest("Username is taken");

            using var hmac = new HMACSHA512();
            
            var user = new AppUser() {
                UserName = registerDto.Username.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                PasswordSalt = hmac.Key                
            };

            context.Users.Add(user);
            await context.SaveChangesAsync();

            return new UserDto {
                Username = user.UserName,
                Token = this._tokenService.CreateToken(user)
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await context.Users.SingleOrDefaultAsync(x=>x.UserName == loginDto.Username);

            if(user == null) return Unauthorized("Invalid Username");

            using var hmac = new HMACSHA512(user.PasswordSalt);
            
            var computerHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            for(int i=0;i<computerHash.Length;i++)
            {
                if(computerHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid Password");
            }

            return new UserDto {
                Username = user.UserName,
                Token = this._tokenService.CreateToken(user)
            };
        }

        private async Task<bool> UserExists(string username)
        {
            return await context.Users.AnyAsync(x=>x.UserName == username.ToLower());
        }
    }
}