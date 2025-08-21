using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Reporting.NETCore;
using VelocityWeb.Services;

namespace VelocityWeb.Pages
{
    public class ReportModel : PageModel
    {
        private readonly IPermissionService _permissionService;
        public string Message { get; set; } = "";

        public ReportModel(IPermissionService permissionService)
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
        }

      
        public IActionResult OnGetPreview()
        {
            LocalReport report = new LocalReport();
            report.ReportPath = Path.Combine(Directory.GetCurrentDirectory(), "Reports", "UserInfo.rdlc");

          
            byte[] pdfBytes = report.Render("PDF");

          
            return File(pdfBytes, "application/pdf"); 
        }
    }
}
