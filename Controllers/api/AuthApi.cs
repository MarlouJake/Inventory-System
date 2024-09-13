using InventorySystem.Data;
using InventorySystem.Models.Accounts;
using InventorySystem.Utilities;
using InventorySystem.Utilities.Api;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using System.Security.Claims;

namespace InventorySystem.Controllers.api
{
    [Route("api/u/")]
    public class AuthApi(ILogger<AuthApi> logger, ApplicationDbContext context) : ControllerBase
    {
#pragma warning disable IDE0052 // Remove unread private members
        private readonly ILogger<AuthApi> _logger = logger;
#pragma warning restore IDE0052 // Remove unread private members
        private readonly ApplicationDbContext _context = context;

        [HttpPost("redirect")]
        public async Task<IActionResult> GetUser([FromBody] LoginModel model)
        {
            try
            {
                // Retrieve user from database (you might want to fetch more details if needed)
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => (u.Username == model.Username || u.Email == model.Username)
                                              && u.Password == HashHelper.HashPassword(model.Password!));

                if (user == null)
                {
                    var data = "Username or Password incorrect";
                    var result = ApiResponseUtils.CustomResponse(false, data, model!);
                    return StatusCode(StatusCodes.Status401Unauthorized, result);
                }

                // Create authentication properties
                var claims = new List<Claim>
                {
                    new(ClaimTypes.Name, user.Username!),
                    new(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                };

                // Add role-based claims
                var roles = await _context.UserRoles
                    .Where(ur => ur.UserId == user.UserId)
                    .Select(ur => ur.Role!.Name)
                    .ToListAsync();

                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role!));

                }

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                // Set the authentication properties
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true // Set to true if you want the user to remain logged in across sessions
                };

                // Sign in the user
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                // Redirect to the user dashboard
                var message = "Authenticated";
                var redirectUrl = Url.Action("UserDashboard", "Users", new { roleName = roles.FirstOrDefault()!, username = user.Username });
                var response = ApiResponseUtils.SuccessResponse(true, message, redirectUrl!);
                return StatusCode(StatusCodes.Status200OK, response);
            }
            catch (MySqlException sqlEx)
            {
                var errorMessage = "An error occurred while connecting to the MySQL database.";

                #region --Console Logger--
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Error.WriteLine($"\nMySQL Exception Caught: {sqlEx}\n");
                Console.ResetColor();
                #endregion

                var response = ApiResponseUtils.CustomResponse(false, errorMessage, null);
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
            catch (Exception ex)
            {
                var errorMessage = "An unknown error occurred.";

                #region --Console Logger--
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Error.WriteLine($"\nException Caught: {ex}\n");
                Console.ResetColor();
                #endregion

                var response = ApiResponseUtils.CustomResponse(false, errorMessage, null);
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

    }
}
