using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VelocityAPI.Model;
using VelocityShared;

namespace VelocityAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionController : ControllerBase
    {
        private readonly PermissionDbContext _context;

        public PermissionController(PermissionDbContext context)
        {
            _context = context;
        }


        [HttpGet("getusers")]
        public IActionResult GetUsers()
        {
            return Ok(_context.Users.ToList());
        }


        [HttpGet("get-by-user")]
        public async Task<ActionResult<List<PermissionInfo>>> GetByUserAsync(int userId)
        {
            var permissions = await _context.Permissions
                .Include(p => p.Page)
                .Where(p => p.UserId == userId)
                .Select(p => new PermissionInfo
                {
                    UserId = p.UserId,
                    PageId = p.PageId,
                    PageTitle = p.Page.PageTitle,
                    PageUrl = p.Page.PageUrl.ToLower() // canonical lowercase
                }).ToListAsync();

            return Ok(permissions);
        }




    }
}