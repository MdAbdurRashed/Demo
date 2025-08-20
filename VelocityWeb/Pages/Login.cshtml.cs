
 using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;
using VelocityShared;
using VelocityWeb.Services;
using VelocityWeb.ViewModel;

    namespace VelocityWeb.Pages
    {
    public class LoginModel : PageModel
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly string _baseUrl;

        public string Message { get; set; } = "";

        public LoginModel(IHttpClientFactory clientFactory, IOptions<ApiSettings> apiSettings)
        {
            _clientFactory = clientFactory;
            _baseUrl = apiSettings.Value.BaseUrl.TrimEnd('/');
        }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            var username = Request.Form["Username"];
            var password = Request.Form["Password"];

            // Call API to get users
            var client = _clientFactory.CreateClient();
            var users = await client.GetFromJsonAsync<List<User>>($"{_baseUrl}/api/permission/getusers");

            var user = users?.FirstOrDefault(u => u.Username == username && u.Password == password);
            if (user != null)
            {
                // Store user info in session
                HttpContext.Session.SetInt32("UserId", user.Id);
                HttpContext.Session.SetString("Username", user.Username);

                // Optional: Load permissions to Redis immediately
                //var permissionService = HttpContext.RequestServices.GetRequiredService<IPermissionService>();
                //await permissionService.LoadPermissionsAsync(user.Id);

                return RedirectToPage("/Index");
            }

            Message = "Invalid credentials";
            return Page();
        }
    }


}


