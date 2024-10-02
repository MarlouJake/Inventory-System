using InventorySystem.Models.Responses;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace InventorySystem.Utilities.Api
{
    /// <summary>
    /// Provides utility methods for creating various types of API responses.
    /// </summary>
    public class ApiResponseUtils
    {
        /// <summary>
        /// Creates an error API response based on the provided <see cref="ModelStateDictionary"/>.
        /// </summary>
        /// <param name="model">The data or model context to include in the response. This represents the result of the operation.</param>
        /// <returns>An <see cref="ApiResponse"/> object containing the error details, including a combined error message from the <see cref="ModelStateDictionary"/>.</returns>
        public static ApiResponse ErrorResponse(object model, string message)
        {
            return new ApiResponse
            {
                IsValid = false,
                Message = message,
                Model = model,
                RedirectUrl = null
            };
        }

        /// <summary>
        /// Creates a successful API response.
        /// </summary>
        /// <param name="model">The data or model context to include in the response. This represents the result of the operation.</param>
        ///  /// <param name="message">A custom message to include in the response, providing additional context or details.</param>
        /// <param name="redirectTo">The URL to which the client should be redirected upon success. This can be used for navigating to another page or resource.</param>
        /// <returns>An <see cref="ApiResponse"/> object containing the result of the operation, including the success status, message, data, and redirect URL.</returns>
        public static ApiResponse SuccessResponse(object model, string message, string redirectTo)
        {
            return new ApiResponse
            {
                IsValid = true,
                Message = message,
                Model = model, // data
                RedirectUrl = redirectTo
            };
        }

        /// <summary>
        /// Creates a customizable API response based on the provided parameters.
        /// </summary>
        /// <param name="validate">A boolean value indicating whether the operation is valid or not.</param>
        /// <param name="message">A custom message to include in the response, providing additional context or details.</param>
        /// <param name="model">The data or model context to include in the response. This represents the result of the operation.</param> 
        /// <returns>An <see cref="ApiResponse"/> object containing the results of the operation, including validity status, custom message, data, and redirect URL (if any).</returns>
        public static ApiResponse CustomResponse(bool validate, string message, object? model = null)
        {
            return new ApiResponse
            {
                IsValid = validate,
                Message = message,
                Model = model, // data
                RedirectUrl = null
            };
        }



    }
}
