using System.ComponentModel.DataAnnotations;

namespace InventorySystem.Models.DataEntities
{
    public class LoginData
    {
        [Key]
        public int Id { get; set; }
        public string? Action { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? BrowserDetails { get; set; }
        public DateTime TimeStamp { get; set; }

    }
}
