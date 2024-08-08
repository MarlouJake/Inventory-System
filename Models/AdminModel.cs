using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace InventorySystem.Models
{
    public class AdminModel
    {

        [DisplayName("Admin ID")]
        public int AdminId { get; set; }

        [MinLength(3, ErrorMessage = "Username must be at least 3 characters long.")]
        [Required(ErrorMessage = "Username or Email is required!")]
        [DisplayName("Username")]
        public string? Username { get; set; }

        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        [Required(ErrorMessage = "Password is required!")]
        [DisplayName("Password")]
        public string? Password { get; set; }
    }
}
