using Api.Data;
using Api.Entiities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{
   
    public class UsersController : BaseApiController
    {
         private readonly DataContext _context;
        public UsersController(DataContext context)
        {
          _context=context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
        {
           
            return await _context.Users.ToListAsync();
        }
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<AppUser>> GetUsers(int id)
        {
           
            return await _context.Users.FindAsync(id);
        }
    }
}