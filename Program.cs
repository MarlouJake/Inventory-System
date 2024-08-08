using InventorySystem.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("RequireAdministratorRole", policy => policy.RequireRole("Administrator"));

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddRoles<IdentityRole>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Home/Index";
    options.AccessDeniedPath = "/Users/AdminView/Admin";
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Apply CORS policy
app.UseCors("AllowAllOrigins");

app.UseAuthentication();
app.UseAuthorization();

// Define routes
app.MapControllerRoute(
    name: "userdashboard",
    pattern: "dashboard/user/{id?}",
    defaults: new { controller = "Users", action = "UserDashboard" });

app.MapControllerRoute(
    name: "additem",
    pattern: "/dashboard/add-item/{id?}",
    defaults: new { controller = "Users", action = "AddItem" });

app.MapControllerRoute(
    name: "delete",
    pattern: "dashboard/delete-item/{id?}",
    defaults: new { controller = "Users", action = "Delete" });

app.MapControllerRoute(
    name: "update",
    pattern: "dashboard/update-item/{id?}",
    defaults: new { controller = "Users", action = "Update" });

app.MapControllerRoute(
    name: "viewdetails",
    pattern: "dashboard/view-item-details/{id?}",
    defaults: new { controller = "Users", action = "ViewDetails" });


app.MapControllerRoute(
    name: "adminviewer",
    pattern: "admin/dashboard/admins",
    defaults: new { controller = "AdminViewer", action = "AdminList" });

app.MapControllerRoute(
    name: "adminlist",
    pattern: "admin/dashboard/admin-list",
    defaults: new { controller = "AdminListView", action = "AdminList" });

app.MapControllerRoute(
    name: "index",
    pattern: "home/login/user",
    defaults: new { controller = "Home", action = "Index" });

app.MapControllerRoute(
    name: "loginpage",
    pattern: "home/login/user",
    defaults: new { controller = "Home", action = "LoginPage" });

app.MapControllerRoute(
    name: "adminlogin",
    pattern: "home/login/admin",
    defaults: new { controller = "Home", action = "AdminLogin" });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Users}/{action=UserDashboard}/{id?}");

app.Run();
