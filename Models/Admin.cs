using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace InventorySystem.Models
{
    public class Admin
    {
        [Key]
        [DisplayName("Admin Id")]
        public int AdminId { get; set; }

        [Required(ErrorMessage = "Username is required")]
        [MinLength(3, ErrorMessage = "Username must atleast be 3 characters long")]
        [DisplayName("Username")]
        public string? Username { get; set; }


        [Required(ErrorMessage = "Password is required")]
        [MinLength(8, ErrorMessage = "Password must be 8 characters long")]
        [DisplayName("Password")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Email is required")]
        [DisplayName("Email")]
        public string? Email { get; set; }

        [DisplayName("Date Created")]
        [DisplayFormat(DataFormatString = "{0:MMMM dd, yyyy hh:mm:ss tt}", ApplyFormatInEditMode = true)]
        public DateTime DateCreated { get; set; }

        public Admin()
        {
            DateCreated = DateTime.Now;
        }
    }
}
