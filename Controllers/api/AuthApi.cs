using InventorySystem.Data;
using InventorySystem.Models.Accounts;
using InventorySystem.Utilities.Api;
using InventorySystem.Utilities.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using System.Security.Claims;

namespace InventorySystem.Controllers.api
{
    [Route("api/u/")]
    [ApiController]
    public class AuthApi(ILogger<AuthApi> logger, ApplicationDbContext context) : ControllerBase
    {
#pragma warning disable IDE0052 // Remove unread private members
        private readonly ILogger<AuthApi> _logger = logger;
#pragma warning restore IDE0052 // Remove unread private members
        private readonly ApplicationDbContext _context = context;

        [HttpPost("redirect")]
        public async Task<IActionResult> GetUser([FromBody] LoginModel model)
        {
            string message;
            try
            {
                // Retrieve user from database (you might want to fetch more details if needed)
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => (u.Username == model.Username || u.Email == model.Username)
                                              && u.Password == HashHelper.HashString(model.Password!));

                if (user == null)
                {
                    message = "Username or Password incorrect";
                    var result = ApiResponseUtils.CustomResponse(false, message, model!);
                    return StatusCode(StatusCodes.Status401Unauthorized, result);
                }

                // Create authentication properties
                var claims = new List<Claim>
                {
                    new(ClaimTypes.Name, user.Username!),
                    new(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                };

                // search for role and add it
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
                message = "Authenticated";
                var redirectUrl = Url.Action("UserDashboard", "Users", new { roleName = roles.FirstOrDefault()!, username = user.Username });
                var response = ApiResponseUtils.SuccessResponse(true, message, redirectUrl!);
                return StatusCode(StatusCodes.Status200OK, response);
            }
            catch (MySqlException sqlEx)
            {
                var errorMessage = "An error occurred while connecting to the server.";

                #region --Console Logger--
                Console.Error.WriteLine($"\nMySQL Exception Caught: {sqlEx}\n");
                #endregion

                var response = ApiResponseUtils.CustomResponse(false, errorMessage, null);
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
            catch (Exception ex)
            {
                var errorMessage = "Username or Password incorrect";

                #region --Console Logger--
                Console.Error.WriteLine($"\nException Caught: {ex}\n");
                #endregion

                var response = ApiResponseUtils.CustomResponse(false, errorMessage, null);
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

    }
}
