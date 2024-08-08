using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace InventorySystem.Models
{
    public class LoginModel
    {
        [DisplayName("User ID")]
        public int UserID { get; set; }

        [MinLength(3, ErrorMessage = "Username must be at least 3 characters long.")]
        [Required(ErrorMessage = "Username or Email is required!")]
        [DisplayName("Username")]
        public string? UserName { get; set; }

        /*
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        [Required(ErrorMessage = "This field is required!")]
        [DisplayName("Email")]
        public string? Email { get; set; }
        */

        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        [Required(ErrorMessage = "Password is required!")]
        [DisplayName("Password")]
        public string? Password { get; set; }
    }
}
