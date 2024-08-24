using InventorySystem.Data;
using InventorySystem.Models;
using InventorySystem.Utilities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;


namespace InventorySystem.Controllers.api
{
    [Route("api/authenticate/")]
    [ApiController]
    public class AuthenticateApi(ApplicationDbContext context) : ControllerBase
    {
        private readonly ApplicationDbContext _context = context;

        public void PrintUrl(string? url)
        {
            if (url != null)
            {
                Console.WriteLine($"API URL: {url}");
            }
            else
            {
                Console.WriteLine("API is null.");
            }
        }

        [HttpPost("user-login")]
        public async Task<IActionResult> LoginPage([FromBody] LoginModel model)
        {

            var errresponse = new
            {
                isValid = false,
                redirectUrl = (string?)null,
                successMessage = (string?)null,
                errorMessage = "There are validation errors. Please check your input."
            };
            try
            {
                if (!ModelState.IsValid)
                {
                    return Ok(new
                    {
                        isValid = false,
                        redirectUrl = (string?)null,
                        successMessage = (string?)null,
                        errorMessage = "Please check your input."
                    });
                }

                if (ModelState.IsValid)
                {
                    if (string.IsNullOrEmpty(model.Password))
                    {
                        return Ok(new
                        {
                            isValid = false,
                            redirectUrl = (string?)null,
                            successMessage = (string?)null,
                            errorMessage = "Password cannot be empty"
                        });
                    }

                    var user = await _context.Users
                       .FirstOrDefaultAsync(u => (u.Username == model.Username || u.Email == model.Username) && u.Password == HashHelper.HashPassword(model.Password));



                    if (user != null)
                    {


                        if (user.Username == null)
                        {
                            return Ok(new
                            {
                                isValid = false,
                                redirectUrl = (string?)null,
                                successMessage = (string?)null,
                                errorMessage = "User not found"
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

                        //TempData["WelcomeMessage"] = $"Welcome, {user.Username}!";

                        var redirectUrl = Url.Action("UserDashboard", "Users", new { username = identity.Name });

                        PrintUrl(redirectUrl);



                        return Ok(new
                        {
                            isValid = true,
                            redirectUrl,
                            successMessage = $"Welcome, {user.Username}!",
                            errorMessage = (string?)null
                        });
                    }

                }

                return Ok(new
                {
                    isValid = false,
                    redirectUrl = (string?)null,
                    successMessage = (string?)null,
                    errorMessage = "Username or Password incorrect"
                });

            }
            catch
            {
                return Ok(new
                {
                    isValid = false,
                    redirectUrl = (string?)null,
                    successMessage = (string?)null,
                    errorMessage = "Username or Password incorrect"
                });

            }


        }


        [HttpPost("admin-login")]
        public async Task<IActionResult> AdminLogin([FromBody] AdminModel model)
        {
            try
            {
                if (!ModelState.IsValid || string.IsNullOrEmpty(model.Password))
                {
                    var failedMessage = string.IsNullOrEmpty(model.Password) ? "Make sure to enter valid credentials!" : "Attempt failed, try again!";
                    return new JsonResult(new
                    {
                        isValid = false,
                        failedMessage
                    });
                }

                var admin = await _context.Admins
                    .FirstOrDefaultAsync(a => (a.Username == model.Username || a.Email == model.Username) && a.Password == HashHelper.HashPassword(model.Password));

                if (admin != null)
                {
                    if (admin.Username == null)
                    {
                        return new JsonResult(new
                        {
                            isValid = false,
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

                    var adminApiUrl = Url.Action("AdminLogin", "AuthenticateApi", new { controller = "AuthenticateApi" }, Request.Scheme);
                    PrintUrl(adminApiUrl);

                    /*
                    return new JsonResult(new
                    {
                        isValid = true,
                        redirectUrl = Url.Action("AdminViewer", "AdminList"),
                        successMessage = "Login Successful!"
                    });*/

                    return Ok(new
                    {
                        isValid = true,
                        redirectUrl = "/dashboard",
                        successMessage = "Login Successful!"
                    });
                }
                else
                {
                    return new JsonResult(new
                    {
                        isValid = false,
                        failedMessage = "Username or Password incorrect!"
                    });
                }
            }
            catch
            {
                return new JsonResult(new
                {
                    isValid = false,
                    failedMessage = "Username or Password incorrect!"
                });
            }
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            var logOutUrl = Url.Action("LoginPage", "Home");
            var returnUrl = Url.Action("Index", "Home");
            // Print the URL to the console
            Console.WriteLine($"Login page URL: {logOutUrl}");
            //return Redirect("/");
            return new JsonResult(new
            {
                isValid = true,
                redirectUrl = returnUrl,
                successMessage = "Logout Successful!"
            });
        }

    }

}

