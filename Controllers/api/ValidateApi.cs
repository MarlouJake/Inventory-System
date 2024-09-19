using InventorySystem.Data;
using InventorySystem.Models.Accounts;
using InventorySystem.Models.DataEntities;
using InventorySystem.Models.Services;
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
    public class ValidateApi(ILogger<ValidateApi> logger, ApplicationDbContext context) : ControllerBase
    {
        private readonly ILogger<ValidateApi> _logger = logger;
        private readonly ApplicationDbContext _context = context;

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
        [HttpPost("add/{id?}")]
        public async Task<IActionResult> AddItem([FromBody] Item model, int? id)
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

                #region --Console Logger--
                Console.WriteLine(response);
                #endregion

                return await Task.FromResult(StatusCode(StatusCodes.Status400BadRequest, response));
            }
            else
            {
                var redirectUrl = Url.Action("AppendItem", "ServicesApi");
                var message = "Validation Successful. Redirecting...";

                #region --Console Logger--
                Console.WriteLine(Messages.PrintUrl(redirectUrl));
                #endregion

                var responseSuccess = ApiResponseUtils.SuccessResponse(model.ItemName!, message, redirectUrl!);
                return await Task.FromResult(StatusCode(StatusCodes.Status202Accepted, responseSuccess));
            }
        }

        [Authorize]
        [HttpPost("modify/{id?}")]
        public async Task<IActionResult> UpdateItem([FromBody] Item model, int? id)
        {
            Console.WriteLine($"model item id: {id}");
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                  .SelectMany(v => v.Errors)
                  .Select(e => e.ErrorMessage)
                  .ToList();

                // Combines the error messages into a single message string
                var message = string.Join("; ", errors);

                var response = ApiResponseUtils.ErrorResponse(null!, message);

                #region --Console Logger--
                Console.WriteLine("Response: ", response);
                Console.WriteLine("Message: ", message);
                Console.WriteLine($"model item id error: {id}");
                #endregion

                return await Task.FromResult(StatusCode(StatusCodes.Status400BadRequest, response));
            }
            else
            {
                var redirectUrl = Url.Action("ModifyItem", "ServicesApi");
                var message = "Validation Successful. Redirecting...";

                #region --Console Logger--
                Console.WriteLine(Messages.PrintUrl(redirectUrl));
                #endregion

                var responseSuccess = ApiResponseUtils.SuccessResponse(model.ItemId!, message, redirectUrl!);
                return await Task.FromResult(StatusCode(StatusCodes.Status202Accepted, responseSuccess));
            }
        }

        [Authorize]
        [HttpPost("remove/{id}")]
        public async Task<IActionResult> DeleteItem([FromBody] int? id)
        {
            string message = "";
            //int Status404 = StatusCodes.Status404NotFound;
            int Status500 = StatusCodes.Status500InternalServerError;

            try
            {
                var redirectUrl = Url.Action("RemoveItem", "ServicesApi", new { id }, Request.Scheme);
                message = "Validation Successful. Redirecting...";
                var response = ApiResponseUtils.SuccessResponse(id!, message, redirectUrl!);
                return await Task.FromResult(StatusCode(StatusCodes.Status202Accepted, response));
            }
            catch (Exception ex)
            {
                Console.Clear();
                Console.WriteLine(ex);
                message = "An unknown error occurred.";
                var response = ApiResponseUtils.ErrorResponse(null!, message);
                return await Task.FromResult(StatusCode(Status500, response));
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
