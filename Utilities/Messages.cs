namespace InventorySystem.Utilities
{
    /// <summary>
    /// Provides a centralized location for commonly used messages throughout the application.
    /// This helps ensure consistency in messaging and makes it easier to manage and update
    /// messages in one place.
    /// </summary>
    public class Messages
    {
        /// <summary>
        /// Error message indicating that a required field is empty or null.
        /// </summary>
        public const string NullOrEmpty = "Required field cannot be empty";

        /// <summary>
        /// Error message indicating that a field contains spaces, which is not allowed.
        /// </summary>
        public const string ContainsSpaces = "Field cannot contains spaces";

        /// <summary>
        /// Error message indicating that the requested data was not found.
        /// </summary>
        public const string NotFound = "The requested data was not found.";

        /// <summary>
        /// Success message indicating that an item has been added successfully.
        /// </summary>
        public const string AddSuccess = "Succesfully added";

        /// <summary>
        /// Error message indicating that the item code already exists in the system.
        /// </summary>
        public const string ItemCodeExists = "Item Code already exists";

        /// <summary>
        /// Error message indicating that the item addition operation failed.
        /// </summary>
        public const string AddFailed = "Failed to add item";

        /// <summary>
        /// Error message indicating that the user ID provided is invalid.
        /// </summary>
        public const string InvalidUserID = "User ID invalid";

        /// <summary>
        /// Error message indicating that the email address format provided is invalid.
        /// </summary>
        public const string FomatEmail = "Email address format invalid";

        /// <summary>
        /// Error message prompting the user to choose a status.
        /// </summary>
        public const string ChooseStatus = "Please choose status";

        /// <summary>
        /// Error message indicating that the maximum allowed length for a username has been reached.
        /// </summary>
        public const string MaxCharLenghtReached = "Username must not exceed 30 characters";

        /// <summary>
        /// Error message indicating that the minimum required length for a username has not been met.
        /// </summary>
        public const string MinimumCharLengthNotReached = "Username must be atleast 3 characters";

        /// <summary>
        /// Error message indicating that the username or password provided is incorrect.
        /// </summary>
        public const string UsernameOrPasswordIncorrect = "Username or Password incorrect";

        /// <summary>
        /// Error message indicating that the maximum allowed length for a username or email has been reached.
        /// </summary>
        public const string MaximumUsernameEmailReached = "Username or Email must not exceed 254 characters";

        /// <summary>
        /// Error message indicating that the maximum allowed length for a password has been reached.
        /// </summary>
        public const string MaximumPasswordReached = "Password must not exceed 128 characters";

        /// <summary>
        /// Formats and returns a URL string. If the provided URL is null, a default message is returned.
        /// </summary>
        /// <param name="url">The URL to format. Can be null.</param>
        /// <returns>A formatted string containing the URL, or a message indicating that the URL is null.</returns>
        public static string PrintUrl(string? url)
        {
            if (url != null)
            {
                return $"URL: {url}";
            }
            else
            {
                return "URL is null.";
            }
        }
    }
}
