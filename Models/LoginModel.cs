using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace InventorySystem.Models
{
    public class LoginModel
    {

        [DisplayName("User ID")]
        public int UserID { get; set; }
        //[MinLength(3, ErrorMessage = "Username must be at least 3 characters long.")]
        [Required(ErrorMessage = "Username or Email is required!")]
        [DisplayName("Username")]
        [JsonPropertyName("username")]
        public string? Username { get; set; }

        // [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        [Required(ErrorMessage = "Password is required!")]
        [DisplayName("Password")]
        [JsonPropertyName("password")]
        public string? Password { get; set; }
    }
}
