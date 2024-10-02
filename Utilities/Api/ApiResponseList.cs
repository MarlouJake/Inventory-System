using InventorySystem.Models.Responses;

namespace InventorySystem.Utilities.Api
{
    /// <summary>
    /// Provides utility methods for creating various types of API message list responses.
    /// </summary>
    public class ApiResponseList
    {
        /// <summary>
        /// Creates an instance of <see cref="ApiMessageListResponse"/> with error messages.
        /// </summary>
        /// <param name="validate">Indicates whether the validation was successful. Default is <c>false</c>.</param>
        /// <param name="messages">A list of error messages. If <c>null</c>, no messages will be included.</param>
        /// <param name="model">An optional model object that may contain additional data related to the error.</param>
        /// <returns>A new instance of <see cref="ApiMessageListResponse"/> containing the validation status, error messages, model, and a <c>null</c> redirect URL.</returns>
        public static ApiMessageListResponse MessageListResponse(bool validate = false, List<string> messages = null!, object? model = null)
        {
            return new ApiMessageListResponse
            {
                IsValid = validate,
                Message = messages,
                Model = model,
                RedirectUrl = null
            };
        }
    }
}
