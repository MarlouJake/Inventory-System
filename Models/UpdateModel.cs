using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace InventorySystem.Models
{
    public class UpdateModel
    {
        [DisplayName("User ID")]
        public int UserId { get; set; }
        [MinLength(3, ErrorMessage = "Username must be at least 3 characters long.")]
        [StringLength(20, ErrorMessage = "Username cannot be longer than 20 characters.")]
        [Required(ErrorMessage = "This field is required!")]
        [DisplayName("Username")]
        public string? Username { get; set; }



        [Required(ErrorMessage = "This field is required!")]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        [DisplayName("Email")]
        public string? Email { get; set; }

        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        //[Required(ErrorMessage = "This field is required!")]
        [DisplayName("Password")]
        public string? Password { get; set; }


        [DisplayName("Date Created")]
        [DisplayFormat(DataFormatString = "{0:MMMM dd, yyyy hh:mm:ss tt}", ApplyFormatInEditMode = true)]
        public DateTime DateCreated { get; set; }

    }
}
