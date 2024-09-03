using InventorySystem.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Access configuration from the builder
var configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddHttpClient();

builder.Services.AddControllersWithViews();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"))
    .AddPolicy("UserOnly", policy => policy.RequireRole("User"));

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.AccessDeniedPath = "/Home/AccessDenied"; // Path for access denied
        options.ExpireTimeSpan = TimeSpan.FromMinutes(15);
        options.Cookie.SameSite = SameSiteMode.Strict;
    });

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // session timeout duration
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseCors("AllowAll"); // Apply CORS policy here

app.UseSession();

app.UseEndpoints(endpoints =>
{
    _ = endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    // Redirect root URL to /login/user
    _ = endpoints.MapGet("/", context =>
    {
        context.Response.Redirect("/home/login");
        return Task.CompletedTask;
    });


    _ = endpoints.MapControllerRoute(
         name: "api-authenticate-user-login",
         pattern: "api/authenticate/user-login",
         defaults: new { controller = "AuthenticateApi" });

    _ = endpoints.MapControllerRoute(
         name: "api-authenticate-admin-login",
         pattern: "api/authenticate/admin-login",
         defaults: new { controller = "AuthenticateApi" });



    _ = endpoints.MapControllerRoute(
      name: "admin-dashboard",
      pattern: "admin-dashboard/{username}",
      defaults: new { controller = "AdminList" });
});

app.Run();
