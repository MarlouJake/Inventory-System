using InventorySystem.Models.Accounts;
using InventorySystem.Utilities;
using InventorySystem.Utilities.Api;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventorySystem.Controllers.api
{
    [Route("api/u/validate/")]
    [ApiController]
    public class ValidateApi : ControllerBase
    {
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                // Extracts error messages from ModelState
                var errors = ModelState.Values
                  .SelectMany(v => v.Errors)
                  .Select(e => e.ErrorMessage)
                  .ToList();

                // Combines the error messages into a single message string
                var message = string.Join("; ", errors);

                var response = ApiResponseUtils.ErrorResponse(null!, message);
                return await Task.FromResult(StatusCode(StatusCodes.Status400BadRequest, response));
            }
            else
            {
                var redirectUrl = Url.Action("GetUser", "AuthApi");
                var message = "Validation Successful. Redirecting...";

                #region --Console Logger--
                Console.Clear();
                Console.WriteLine(Messages.PrintUrl(redirectUrl));
                #endregion

                var responseSuccess = ApiResponseUtils.SuccessResponse(model.Username!, message, redirectUrl!);
                return await Task.FromResult(StatusCode(StatusCodes.Status202Accepted, responseSuccess));
            }
        }


        [Authorize]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            Response.Cookies.Delete(CookieAuthenticationDefaults.AuthenticationScheme);

            var logOutUrl = Url.Action("LoginPage", "Home");
            var returnUrl = Url.Action("Index", "Home");

            // Print the URL to the console
            Console.WriteLine(Messages.PrintUrl(logOutUrl));

            return new JsonResult(new
            {
                isValid = true,
                redirectUrl = returnUrl,
                successMessage = "Logout Successful!"
            });
        }
    }
}
