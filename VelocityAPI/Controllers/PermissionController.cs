using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VelocityAPI.Model;

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


        [HttpGet("check")]
        public IActionResult CheckPermission(int userId, int pageId)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
                return NotFound("User not found");

            var hasAccess = _context.Permissions.Any(p => p.UserId == userId && p.PageId == pageId);

            return Ok(new
            {
                UserId = userId,
                PageId = pageId,
                AccessGranted = hasAccess
            });
        }

        [HttpGet("check-by-url")]
        public IActionResult CheckPermissionByUrl(int userId, string pageUrl)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
                return NotFound("User not found");

            var hasAccess = _context.Permissions
                .Any(p => p.UserId == userId && p.Page.PageUrl == pageUrl);

            return Ok(new
            {
                UserId = userId,
                PageUrl = pageUrl,
                AccessGranted = hasAccess
            });
        }



    }
}