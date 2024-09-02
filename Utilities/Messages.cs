namespace InventorySystem.Utilities
{
    public static class Messages
    {
        public const string NullOrEmpty = "Required field cannot be empty";
        public const string ContainsSpaces = "Field cannot contains spaces";
        public const string NotFound = "The requested data was not found.";



        public const string AddSuccess = "Succesfully added";

        public const string ItemCodeExists = "Item Code already exists";
        public const string AddFailed = "Failed to add item";
        public const string InvalidUserID = "User ID invalid";
        public const string ChooseStatus = "Please choose status";

        public static string PrintUrl(string? url)
        {
            if (url != null)
            {
                return $"API URL: {url}";
            }
            else
            {
                return "API is null.";
            }
        }
    }
}
