using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.DTOS;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;
        public AccountController(DataContext context, ITokenService tokenService)
        {
            _tokenService = tokenService;
            _context = context;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto){
            // validation is added in DTO with [required]


            // can use BadRequest because of ActionResult
            if(await UserExists(registerDto.UserName)) return BadRequest("Username is Taken");


            // use using because when it is done, it disposes automatically
            using var hmac = new HMACSHA512();

            var user = new AppUser{
                UserName = registerDto.UserName.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                PasswordSalt = hmac.Key
            };

            _context.Users.Add(user);

            await _context.SaveChangesAsync();

            return new UserDto{
                UserName = user.UserName,
                Token = _tokenService.CreateToken(user)
            };
        }


        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto){
            var user = await _context.Users.SingleOrDefaultAsync(x => x.UserName == loginDto.UserName);

            if(user == null) return Unauthorized("Invalid username");

            // user password salt is the Hmac key
            using var hmac = new HMACSHA512(user.PasswordSalt);

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));
            // loop over the bytes of computed hash
            for(int i =0; i < computedHash.Length; i++){
                // if computed hash [i] is not equal to passwordHash[i] then the password was entered wrong
                if(computedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid Password");
            }

            return new UserDto{
                UserName = user.UserName,
                Token = _tokenService.CreateToken(user)
            };
        }

        private async Task<bool> UserExists(string username){
            return await _context.Users.AnyAsync(x => x.UserName == username.ToLower());
        }
    }
}