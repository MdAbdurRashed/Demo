using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VelocityShared;
using VelocityWeb.Services;

namespace VelocityWeb.Pages
{
    public class CarModel : PageModel
    {
        private readonly IPermissionService _permissionService;
        public string Message { get; set; } = "";

        public CarModel(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        public async Task OnGetAsync()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                Message = "Please login first.";
                return;
            }

            //var permissions = await _permissionService.GetPermissionsAsync(userId.Value);

            //var hasAccess = permissions.Any(p => p.PageId == PageConstants.CarPageId);
            //Message = hasAccess ? "Access granted to Car page" : "Access denied";
        }
    }
}
