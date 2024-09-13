namespace InventorySystem.Models.Responses
{
    public class ApiResponse
    {
        public bool IsValid { get; set; }
        public string? RedirectUrl { get; set; }
        public string? Message { get; set; }
        public object? Model { get; set; }
    }
}
