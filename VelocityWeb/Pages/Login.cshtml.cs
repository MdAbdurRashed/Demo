
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using System.Net.Http.Json;

    namespace VelocityWeb.Pages
    {
    public class LoginModel : PageModel
    {
        private readonly IHttpClientFactory _clientFactory;
        public string Message { get; set; } = "";

        public LoginModel(IHttpClientFactory clientFactory) { _clientFactory = clientFactory; }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            var username = Request.Form["Username"];
            var password = Request.Form["Password"];

            var client = _clientFactory.CreateClient();
            var users = await client.GetFromJsonAsync<List<User>>("https://localhost:44371/api/permission/getusers");

            var user = users?.FirstOrDefault(u => u.Username == username && u.Password == password);
            if (user != null)
            {
                HttpContext.Session.SetInt32("UserId", user.Id);
                HttpContext.Session.SetString("Username", user.Username);
                return RedirectToPage("/Index");
            }

            Message = "Invalid credentials";
            return Page();
        }

        public class User { public int Id { get; set; } public string Username { get; set; } = ""; public string Password { get; set; } = ""; }
    }


}


