using Api.Data;
using Microsoft.AspNetCore.Mvc;
using Api.Entiities;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using Api.DTOs;
using Api.interfaces;

namespace Api.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;
        public AccountController(DataContext context,ITokenService TokenService)
        {
            _context=context;
            _tokenService=TokenService;
        }
         
         [HttpPost("register")]
           public async Task<ActionResult<UserDTo>> Register(RegisterDTo registerDTo)
        {
            if(await UserExists(registerDTo.Username)) return BadRequest("Username is Taken");

                using var hmac=new HMACSHA512();
                var user =new AppUser{
                    Username=registerDTo.Username.ToLower(),
                    PasswordHash=hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDTo.Password)),
                    PasswordSalt=hmac.Key
                };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
                
             return new UserDTo{
                 Username=user.Username,
                 Token=_tokenService.CreateToken(user)                 
             };
        }

        private async Task<bool> UserExists(string username)
        {
             return await _context.Users.AnyAsync(x=>x.Username==username.ToLower());
        }

       [HttpPost("login")]
       public async Task<ActionResult<UserDTo>> Login(LoginDto loginDto)
       {
           var user=await _context.Users.SingleOrDefaultAsync((x=>x.Username==loginDto.Username.ToLower()));
           
               
           if(user==null)
           {
               return Unauthorized("Invalid Username");
           }

             using var hmac=new HMACSHA512(user.PasswordSalt);

             var ComputeHash=hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

             for(int i=0;i<ComputeHash.Length;i++)
             {
                 if(ComputeHash[i]!=user.PasswordHash[i])
                 {
                     return Unauthorized("Invalid Password");
                 }
             }
              return new UserDTo{
                 Username=user.Username,
                 Token=_tokenService.CreateToken(user)                 
             };
       }

    }
}