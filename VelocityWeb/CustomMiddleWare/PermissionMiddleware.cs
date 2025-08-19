using VelocityWeb.Services;

namespace VelocityWeb.CustomMiddleWare
{
    public class PermissionMiddleware
    {
        private readonly RequestDelegate _next;

        public PermissionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IPermissionService permissionService)
        {
            // Normalize path
            var path = (context.Request.Path.Value ?? string.Empty).TrimEnd('/').ToLower();

            // Public pages
            var publicPaths = new[] { "/", "/index", "/login","/logout"}
                              .Select(p => p.TrimEnd('/').ToLower())
                              .ToArray();
            if (publicPaths.Contains(path))
            {
                await _next(context);
                return;
            }

            // Get userId from session
            var userId = context.Session.GetInt32("UserId");
            if (userId == null)
            {
                context.Response.Redirect("/Login");
                return;
            }

            // Load permissions from cache / API
            var permissions = await permissionService.GetPermissionsAsync(userId.Value);

            // Debug logging
            Console.WriteLine($"Request path: {path}");
            Console.WriteLine($"UserId: {userId}, Permissions count: {permissions?.Count}");

            // Check if user has access
            var hasAccess = permissions.Any(p => p.PageUrl.TrimEnd('/').ToLower() == path);

            if (!hasAccess)
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsync("Access Denied");
                return;
            }

            await _next(context);
        }
    }
}
