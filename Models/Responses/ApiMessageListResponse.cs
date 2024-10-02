namespace InventorySystem.Models.Responses
{
    public class ApiMessageListResponse
    {
        public bool IsValid { get; set; }
        public List<string>? Message { get; set; }
        public object? Model { get; set; }
        public string? RedirectUrl { get; set; }
    }
}
