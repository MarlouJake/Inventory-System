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

    public class HomeController(ApplicationDbContext context) : Controller
    {
        //private readonly ILogger<HomeController> _logger = logger;
        private readonly ApplicationDbContext _context = context;

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
                        failedMessage = "Password cannot be empty"
                    });
                }

                var user = await _context.Users
                   .FirstOrDefaultAsync(u => (u.Username == model.Username || u.Email == model.Username) && u.Password == HashHelper.HashPassword(model.Password));



                if (user != null)
                {


                    if (user.Username == null)
                    {
                        return Json(new
                        {
                            isValid = false,
                            html = Helper.RenderRazorViewToString(this, "LoginPage", model),
                            failedMessage = "Username not found!"
                        });
                    }



                    // Set authentication cookie
                    var claims = new List<Claim>
                    {
                        new(ClaimTypes.Name, user.Username),
                        new(ClaimTypes.NameIdentifier, user.UserId.ToString())

                    };

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                    TempData["WelcomeMessage"] = $"Welcome, {user.Username}!";

                    return Json(new
                    {
                        isValid = true,
                        redirectUrl = Url.Action("UserDashboard", "Users", new { username = identity.Name }),
                        successMessage = "Wait for a moment..."
                    });
                }

            }

            return Json(new
            {
                isValid = false,
                html = Helper.RenderRazorViewToString(this, "LoginPage", model),
                failedMessage = "Username or Password incorrect"
            });
        }

        public IActionResult Info()
        {
            return PartialView();
        }

        [HttpGet]
        public IActionResult AdminLogin()
        {
            var admin = new AdminModel();
            return View(admin);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdminLogin(AdminModel model)
        {
            if (!ModelState.IsValid || string.IsNullOrEmpty(model.Password))
            {
                var failedMessage = string.IsNullOrEmpty(model.Password) ? "Make sure to enter valid credentials!" : "Attempt failed, try again!";
                return Json(new
                {
                    isValid = false,
                    html = Helper.RenderRazorViewToString(this, "AdminLogin", model),
                    failedMessage
                });

            }

            var admin = await _context.Admins
                .FirstOrDefaultAsync(a => (a.Username == model.Username || a.Email == model.Username) && a.Password == HashHelper.HashPassword(model.Password));

            if (admin != null)
            {
                if (admin.Username == null)
                {
                    return Json(new
                    {
                        isValid = false,
                        html = Helper.RenderRazorViewToString(this, "LoginPage", model),
                        failedMessage = "Username not found!"
                    });
                }
                var claims = new List<Claim>
                    {
                        new(ClaimTypes.Name, admin.Username),
                        new(ClaimTypes.NameIdentifier, admin.AdminId.ToString())
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
                    failedMessage = "Username or Password incorrect!"
                });
            }


        }

        public IActionResult AccessDenied()
        {
            //return new HttpStatusCodeResult(HttpStatusCode.Forbidden); // 403 Forbidden
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index");
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
