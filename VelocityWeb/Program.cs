using VelocityWeb.CustomMiddleWare;
using VelocityWeb.Services;
using VelocityWeb.ViewModel;

var builder = WebApplication.CreateBuilder(args);

// HttpClient for API calls
builder.Services.AddHttpClient();

// Redis cache
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
});

// Session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(1);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// API settings
builder.Services.Configure<ApiSettings>(builder.Configuration.GetSection("ApiSettings"));

builder.Services.AddSingleton<IPermissionService, PermissionService>();


builder.Services.AddRazorPages();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseSession();
app.UseMiddleware<PermissionMiddleware>();

app.UseAuthorization();
app.MapRazorPages();

app.Run();
