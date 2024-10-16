using InventorySystem.Data;
using InventorySystem.Utilities.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Access configuration from the builder
        var configuration = builder.Configuration;

        // Configure Services
        ConfigureServices(builder.Services, configuration);

        var app = builder.Build();

        // Configure Middleware
        ConfigureMiddleware(app);

        app.Run();
    }

    private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        // Add HTTP Client
        services.AddHttpClient();
        services.AddControllersWithViews();
        // Add Controllers with JSON options
        services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = null;

            });

        services.AddScoped<GetClaims>();
        services.AddScoped<ItemQuery>();
        services.AddScoped<CheckInputs>();
        services.AddScoped<ValidateArrayOfId>();

        // Add CORS policy
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll",
                builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
        });

        // Add DbContext with MySQL
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
        );

        // Configure Authentication
        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.LoginPath = "/home/login";
                options.AccessDeniedPath = "/access-denied";
            });

        // Configure Authorization
        _ = services.AddAuthorization(options =>
        {
            // Add policies if needed, e.g., for roles
            options.AddPolicy("RequireAdministratorRole", policy =>
                policy.RequireRole("Administrator"));
            options.AddPolicy("RequireUserRole", policy =>
                policy.RequireRole("User"));
        });


    }

    private static void ConfigureMiddleware(WebApplication app)
    {
        // Error handling and security configurations
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        // Standard middleware for routing, sessions, etc.
        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();

        // Authentication & Authorization
        app.UseAuthentication();
        app.UseAuthorization();

        // Custom middleware to handle status code
        app.UseStatusCodePages(async context =>
        {
            var response = context.HttpContext.Response;
            var statusCode = response.StatusCode;

            if (statusCode == 503) // Service Unavailable
            {
                response.ContentType = "text/html";
                await response.WriteAsync("<html><body><h1>Service Unavailable</h1><p>The service is temporarily unavailable. Please try again later.</p></body></html>");
                return;
            }


        });

        app.Use(async (context, next) =>
        {
            await next();

            var response = context.Response;

            if (response.StatusCode == 401)
            {
                context.Response.Redirect("/access-denied");
            }
        });


        // CORS policy
        app.UseCors("AllowAll");

        // Session management
        //app.UseSession();

        // Configure routing
        ConfigureEndpoints(app);
    }

    private static void ConfigureEndpoints(WebApplication app)
    {
        app.UseEndpoints(endpoints =>
        {
            // Default MVC route
            _ = endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            // Redirect root URL to login
            _ = endpoints.MapGet("/", context =>
            {
                context.Response.Redirect("/home/login");
                return Task.CompletedTask;
            });

            // API routes
            _ = endpoints.MapControllerRoute(
                name: "api-validate-userlogin",
                pattern: "api/u/validate/login",
                defaults: new { controller = "ValidateApi", action = "Login" });

            _ = endpoints.MapControllerRoute(
                name: "api-validate-add",
                pattern: "api/u/validate/add/{id?}",
                defaults: new { controller = "ValidateApi", action = "AddItem" });

            _ = endpoints.MapControllerRoute(
                name: "api-validate-update",
                pattern: "api/u/validate/modify/{id?}",
                defaults: new { controller = "ValidateApi", action = "UpdateItem" });

            _ = endpoints.MapControllerRoute(
                name: "api-validate-delete",
                pattern: "api/u/validate/remove/{id?}",
                defaults: new { controller = "ValidateApi", action = "DeleteItem" });

            _ = endpoints.MapControllerRoute(
                name: "api-authenticate-authorize",
                pattern: "api/u/redirect",
                defaults: new { controller = "AuthApi", action = "GetUser" });

            _ = endpoints.MapControllerRoute(
                name: "api-u-services-delete",
                pattern: "api/u/services/remove-confirm/{id?}",
                defaults: new { controller = "Services", action = "RemoveConfirm" });



            // Standard dashboard route
            _ = endpoints.MapControllerRoute(
                name: "standard-content",
                pattern: "{roleName}/ims/{username}/content",
                defaults: new { controller = "Users", action = "ContentHandler" })
                .RequireAuthorization("RequireUserRole");

            _ = endpoints.MapControllerRoute(
                  name: "standard-dashboard",
                  pattern: "{roleName}/ims/{username}/dashboard",
                  defaults: new { controller = "Users", action = "Dashboard" })
                  .RequireAuthorization("RequireUserRole");

            _ = endpoints.MapControllerRoute(
                name: "standard-inventory",
                pattern: "{roleName}/ims/{username}/inventory",
                defaults: new { controller = "Users", action = "Inventory" })
                .RequireAuthorization("RequireUserRole");

            _ = endpoints.MapControllerRoute(
               name: "standard-requests",
               pattern: "{roleName}/ims/{username}/requests",
               defaults: new { controller = "Users", action = "Requests" })
               .RequireAuthorization("RequireUserRole");


            //Services route
            _ = endpoints.MapControllerRoute(
              name: "update",
              pattern: "{roleName}/ims/{username}/inventory/update/{id?}",
              defaults: new { controller = "Users", action = "Update" })
              .RequireAuthorization("RequireUserRole");

            _ = endpoints.MapControllerRoute(
            name: "delete",
            pattern: "{roleName}/ims/{username}/inventory/remove/{id?}",
            defaults: new { controller = "Users", action = "Delete" })
            .RequireAuthorization("RequireUserRole");

            _ = endpoints.MapControllerRoute(
            name: "viewdetails",
            pattern: "{roleName}/ims/{username}/inventory/details/{id?}",
            defaults: new { controller = "Users", action = "ViewDetails" })
            .RequireAuthorization("RequireUserRole");

            _ = endpoints.MapControllerRoute(
              name: "searchView",
              pattern: "{roleName}/ims/{username}/inventory/search/",
              defaults: new { controller = "Users", action = "Search" })
              .RequireAuthorization("RequireUserRole");


            //View routes
            _ = endpoints.MapControllerRoute(
              name: "viewAll",
              pattern: "{roleName}/ims/{username}/inventory/items/uncategorized",
              defaults: new { controller = "Users", action = "ItemsView" })
              .RequireAuthorization("RequireUserRole");


            _ = endpoints.MapControllerRoute(
              name: "categoryView",
              pattern: "{roleName}/ims/{username}/inventory/items/categorized",
              defaults: new { controller = "Users", action = "CategoryView" })
              .RequireAuthorization("RequireUserRole");


        });

        app.UseDeveloperExceptionPage();
    }
}
