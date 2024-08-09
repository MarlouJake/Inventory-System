using InventorySystem.Data;
using InventorySystem.Models;
using InventorySystem.Utilities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;


namespace InventorySystem.Controllers
{
    public class HomeController(ILogger<HomeController> logger, ApplicationDbContext context) : Controller
    {
        private readonly ILogger<HomeController> _logger = logger;
        private readonly ApplicationDbContext _context = context;
        public ILogger<HomeController> Logger => _logger;

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult LoginPage()
        {
            var user = new LoginModel();
            return PartialView(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginPage(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(model.Password))
                {
                    return Json(new
                    {
                        isValid = false,
                        html = Helper.RenderRazorViewToString(this, "LoginPage", model),
                        failedMessage = "Password cannot be empty!"
                    });
                }
                var user = await _context.Users
                   .FirstOrDefaultAsync(u => (u.Username == model.Username || u.Email == model.Username) && u.Password == HashHelper.HashPassword(model.Password));

                if (user != null)
                {
                    return Json(new
                    {
                        isValid = true,
                        redirectUrl = Url.Action("UserDashboard", "Users"),
                        successMessage = "Login Successful!"
                    });

                }

                else
                {
                    return Json(new
                    {
                        isValid = false,
                        html = Helper.RenderRazorViewToString(this, "LoginPage", model),
                        failedMessage = "User not found!"
                    });
                }

            }

            return Json(new
            {
                isValid = false,
                html = Helper.RenderRazorViewToString(this, "LoginPage", model),
                failedMessage = "Attempt failed, try again!"
            });
        }


        /*
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginPage(LoginModel model)
        {
            var errorRender = PartialView("Index", model);
            if (ModelState.IsValid)
            {
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => (u.Username == model.UserName || u.Email == model.UserName) && u.Password == HashHelper.HashPassword(model.Password));

                if (user != null)
                {
                    // Set authentication cookie or session here

                    Console.WriteLine("Admin Page");


                    return Json(new
                    {
                        isValid = true,
                        redirectUrl = Url.Action("Admin", "Users"), // Redirect to the admin page                       
                        successMessage = "Login Successful!"
                    });
                }
                else
                {
                    Console.WriteLine("User not found");
                    return Json(new
                    {
                        isValid = false,
                        errorMessage = "Username or Password is incorrect!"
                    });
                }

            }

            // Return the view with the model to show validation errors

            Console.WriteLine("Return Index");
            return Json(new
            {
                isValid = false,
                errorRender,
                errorMessage = "Username or Password is incorrect!"
            });
        }*/

        public IActionResult Info()
        {
            return PartialView();
        }

        [HttpGet]
        public IActionResult AdminLogin()
        {
            var admin = new LoginModel();
            return View(admin);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdminLogin(LoginModel model)
        {

            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(model.Password))
                {
                    return Json(new
                    {
                        isValid = false,
                        html = Helper.RenderRazorViewToString(this, "AdminLogin", model),
                        failedMessage = "Password cannot be empty!"
                    });
                }
                /*
                var user = await _context.Admins
                   .FirstOrDefaultAsync(u => (u.Username == model.Username || u.Email == model.Username) && u.Password == HashHelper.HashPassword(model.Password));
                */
                if ((model.Username == "admin" || model.Username == "admin@admin.admin") && model.Password == "admin1234")
                {

                    var claims = new List<Claim>
                    {
                        new(ClaimTypes.Name, model.Username),
                        // Add additional claims if needed
                    };

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                    return Json(new
                    {
                        isValid = true,
                        redirectUrl = Url.Action("AdminViewer", "AdminList"),
                        successMessage = "Login Successful!"
                    });
                }

                else
                {
                    return Json(new
                    {
                        isValid = false,
                        html = Helper.RenderRazorViewToString(this, "AdminLogin", model),
                        failedMessage = "User not found!"
                    });
                }

            }

            return Json(new
            {
                isValid = false,
                html = Helper.RenderRazorViewToString(this, "AdminLogin", model),
                failedMessage = "Attempt failed, try again!"
            });
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("AdminLogin");
        }


        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
