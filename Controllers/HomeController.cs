using InventorySystem.Data;
using InventorySystem.Models;
using InventorySystem.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;


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
            return PartialView(new LoginModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginPage(LoginModel model)
        {
            var errorRender = PartialView("Index", model);
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(model.Password))
                {
                    Console.WriteLine("Password empty");
                    TempData["ErrorMessage"] = "Password cannot be empty!";
                    return Json(new
                    {
                        isValid = false,
                        errorRender,
                        failedMessage = "Password cannot be empty!"
                    });
                }

                var user = await _context.Users
                    .FirstOrDefaultAsync(u => (u.Username == model.UserName || u.Email == model.UserName) && u.Password == HashHelper.HashPassword(model.Password));

                if (user != null)
                {
                    // Set authentication cookie or session here
                    Console.WriteLine("Admin Page");
                    TempData["SuccessMessage"] = "Login Successful!";
                    return RedirectToAction("Admin", "Users");
                }
            }

            Console.WriteLine("Returned to index");
            TempData["ErrorMessage"] = "Username or Password is incorrect!";
            return RedirectToAction("Admin", "Users");
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
            var admin = new AdminModel();
            return View(admin);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdminLogin(AdminModel model)
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
                var user = await _context.Admins
                   .FirstOrDefaultAsync(u => (u.Username == model.Username || u.Email == model.Username) && u.Password == HashHelper.HashPassword(model.Password));

                if (user != null)
                {
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




        [HttpGet]
        public IActionResult AdminCreate()
        {
            var admin = new Admin();
            return PartialView(admin);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdminCreate([Bind("AdminId, Username, Email, Password, DateCreated")] Admin adminModel)
        {


            adminModel.Username = adminModel.Username?.Trim();
            adminModel.Email = adminModel.Email?.Trim();
            adminModel.Password = adminModel.Password?.Trim();

            if (ModelState.IsValid)
            {

                var usernameExist = await _context.Admins.AnyAsync(u => u.Username == adminModel.Username);
                var emailExist = await _context.Admins.AnyAsync(u => u.Email == adminModel.Email);
                if (usernameExist)
                {
                    return Json(new
                    {
                        isValid = false,
                        html = Helper.RenderRazorViewToString(this, "AdminCreate", adminModel),
                        failedMessage = "Username already exists!"
                    });
                }

                if (emailExist)
                {
                    return Json(new
                    {
                        isValid = false,
                        html = Helper.RenderRazorViewToString(this, "AdminCreate", adminModel),
                        failedMessage = "Email already exists!"
                    });
                }

#pragma warning disable CS8602 // Dereference of a possibly null reference.
                if (adminModel.Username.Contains(' ') || adminModel.Email.Contains(' ') || adminModel.Password.Contains(' '))
                {
                    ModelState.AddModelError("", "Input cannot contain spaces.");
                    return Json(new
                    {
                        isValid = false,
                        html = Helper.RenderRazorViewToString(this, "AdminCreate", adminModel),
                        failedMessage = "Input cannot contain spaces!"
                    });
                }
#pragma warning restore CS8602 // Dereference of a possibly null reference.

                try
                {
                    adminModel.Password = adminModel.Password != null ? HashHelper.HashPassword(adminModel.Password) : string.Empty;
                    _context.Admins.Add(adminModel);
                    await _context.SaveChangesAsync();


                    return Json(new
                    {
                        isValid = true,
                        html = Helper.RenderRazorViewToString(this, "AdminCreate", adminModel),
                        successMessage = "Successfuly created!"
                    });
                    //return RedirectToAction(nameof(Admin));
                    //return Json(new(ModelState.IsValid = true, html = "")); 
                }
                catch (Exception ex)
                {
                    return Json(new
                    {
                        isValid = false,
                        html = Helper.RenderRazorViewToString(this, "AdminCreate", adminModel),
                        failedMessage = $"An error occured while saving: {ex.Message}"
                    });
                }

            }



            var admin = await _context.SaveChangesAsync();

            return Json(new
            {
                isValid = false,
                html = Helper.RenderRazorViewToString(this, "AdminCreate", adminModel),
                failedMessage = "Failed to create user!"
            });
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
