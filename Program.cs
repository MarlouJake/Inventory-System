using InventorySystem.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("RequireAdministratorRole", policy => policy.RequireRole("Administrator"));

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddRoles<IdentityRole>()
    .AddDefaultTokenProviders();
/*
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();*/

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

app.UseAuthentication();
app.UseAuthorization();

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
    pattern: "admin/dashboard/delete-user/{id?}",
    defaults: new { controller = "Users", action = "Delete" });


app.MapControllerRoute(
    name: "update",
    pattern: "admin/dashboard/update-user/{id?}",
    defaults: new { controller = "Users", action = "Update" });

app.MapControllerRoute(
    name: "viewdetails",
    pattern: "admin/dashboard/view-user-details/{id?}",
    defaults: new { controller = "Users", action = "ViewDetails" });



#region --Test MapAreaControllerRoute--

/*
app.MapAreaControllerRoute(
    name: "admin",
    areaName: "admin",
    pattern: "admin/users/view_details/{id?}");

*/

/*
app.MapControllerRoute(
    name: "loginpage",
    pattern: "home/login-page",
    defaults: new { controller = "Home", action = "LoginPage" });*/

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

#endregion
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Users}/{action=UserDashboard}/{id?}");

app.Run();
