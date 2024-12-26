using InventorySystem.Models.Accounts;
using InventorySystem.Models.DataEntities;
using InventorySystem.Models.Responses;
using InventorySystem.Utilities;
using InventorySystem.Utilities.Api;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventorySystem.Controllers.api
{
    [Authorize]
    [Route("api/u/validate/")]
    [ApiController]
    public class ValidateApi : ControllerBase
    {

        [AllowAnonymous]
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
                var message = "Redirecting...";

                #region --Console Logger--
                Console.WriteLine(Messages.PrintUrl(redirectUrl));
                #endregion

                var responseSuccess = ApiResponseUtils.SuccessResponse(model.Username!, message, redirectUrl!);
                return await Task.FromResult(StatusCode(StatusCodes.Status202Accepted, responseSuccess));
            }
        }

        [AllowAnonymous]
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                  .SelectMany(v => v.Errors)
                  .Select(e => e.ErrorMessage)
                  .ToList();

                var message = string.Join("; ", errors);
                Console.WriteLine(message);
                var response = ApiResponseUtils.ErrorResponse(null!, message);
                return await Task.FromResult(StatusCode(StatusCodes.Status400BadRequest, response));
            }
            else
            {
                var redirectUrl = Url.Action("CreateNewAccount", "RegisterApi");
                var message = "Redirecting...";

                #region --Console Logger--
                Console.WriteLine(Messages.PrintUrl(redirectUrl));
                #endregion
                Console.WriteLine(message);
                var responseSuccess = ApiResponseUtils.SuccessResponse(model.Username!, message, redirectUrl!);
                return await Task.FromResult(StatusCode(StatusCodes.Status202Accepted, responseSuccess));
            }
        }

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

        [HttpPost("remove/{id?}")]
        public async Task<IActionResult> DeleteItem([FromBody] int? id)
        {
            string message = "";
            //int Status404 = StatusCodes.Status404NotFound;
            int Status500 = StatusCodes.Status500InternalServerError;

            try
            {
                var redirectUrl = Url.Action("RemoveConfirm", "ServicesApi", new { id }, Request.Scheme);

                message = "Redirecting...";
                var response = ApiResponseUtils.SuccessResponse(id!, message, redirectUrl!);
                return await Task.FromResult(StatusCode(StatusCodes.Status202Accepted, response));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                message = "An unknown error occurred.";
                var response = ApiResponseUtils.ErrorResponse(null!, message);
                return await Task.FromResult(StatusCode(Status500, response));
            }
        }

        [HttpPost("remove-item/{id?}")]
        public async Task<IActionResult> DeleteMultipleItem([FromBody] string[]? ids)
        {
            string message = "";
            //int Status404 = StatusCodes.Status404NotFound;
            int Status500 = StatusCodes.Status500InternalServerError;
            ApiResponse response;
            try
            {
                // Check if any IDs were provided
                if (ids == null || ids.Length == 0)
                {
                    message = "No IDs provided for deletion.";
                    response = ApiResponseUtils.ErrorResponse(null!, message);
                    return await Task.FromResult(StatusCode(StatusCodes.Status400BadRequest, response));
                }

                // Handles the deletion of items with the provided IDs
                foreach (var id in ids)
                {

                    Console.WriteLine($"Deleting item with ID: {id}");
                    // Example: await service.DeleteItemAsync(id);
                }

                var redirectUrl = Url.Action("MultipleRemoveConfirm", "ServicesApi", new { ids }, Request.Scheme);

                message = "Redirecting...";
                response = ApiResponseUtils.SuccessResponse(ids!, message, redirectUrl!);
                return await Task.FromResult(StatusCode(StatusCodes.Status202Accepted, response));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                message = "An unknown error occurred.";
                response = ApiResponseUtils.ErrorResponse(null!, message);
                return await Task.FromResult(StatusCode(Status500, response));
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            // Sign out the user from the cookie authentication scheme
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Optionally, delete the authentication cookie explicitly (usually not needed)
            Response.Cookies.Delete(CookieAuthenticationDefaults.AuthenticationScheme);

            // Redirect URLs after logout
            var logOutUrl = Url.Action("LoginPage", "Home");
            var returnUrl = Url.Action("Index", "Home");

            // Print the URL to the console for debugging
            Console.WriteLine(Messages.PrintUrl(logOutUrl));

            var message = "Logout Successful!";
            var response = ApiResponseUtils.SuccessResponse(null!, message, returnUrl!);
            return await Task.FromResult(StatusCode(StatusCodes.Status200OK, response));
        }
    }
}
