using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace VelocityWeb.Pages
{
    public class BusModel : PageModel
    {
        private readonly IHttpClientFactory _clientFactory;
        public string Message { get; set; } = "";

        public BusModel(IHttpClientFactory clientFactory) { _clientFactory = clientFactory; }

        public async Task OnGetAsync()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) { Message = "Please login first."; return; }

            var client = _clientFactory.CreateClient();
            var response = await client.GetFromJsonAsync<PermissionResponse>($"https://localhost:44371/api/permission/check?userId={userId}&pageId=1");
            Message = response != null && response.AccessGranted ? "Access granted to Bus page" : "Access denied";
        }

        public class PermissionResponse { public int UserId { get; set; } public int PageId { get; set; } public bool AccessGranted { get; set; } }
    }

    
}
