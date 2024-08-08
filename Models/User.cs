using InventorySystem.Attributes;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventorySystem.Models
{
    public class User
    {

        [DisplayName("User ID")]
        [Key]
        public int UserId { get; set; }

        [ForeignKey("AdminId")]
        [DisplayName("Admin ID")]
        public int AdminId { get; set; }

        [MinLength(3, ErrorMessage = "Username must be at least 3 characters long.")]
        [Required(ErrorMessage = "Username cannot be empty!")]
        [DisplayName("Username")]
        [NoSpaces(ErrorMessage = "Username cannot contain spaces.")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "Email is required!")]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        [DisplayName("Email")]
        [NoSpaces(ErrorMessage = "Username cannot contain spaces.")]
        public string? Email { get; set; }

        [ConditionalRequired("IsCreationContext")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        //[Required(ErrorMessage = "This field is required!")]
        [DisplayName("Password")]
        [NoSpaces(ErrorMessage = "Password cannot contain spaces.")]
        public string? Password { get; set; }


        [DisplayName("Date Created")]
        [DisplayFormat(DataFormatString = "{0:MMMM dd, yyyy hh:mm:ss tt}", ApplyFormatInEditMode = true)]
        public DateTime DateCreated { get; set; }

        public User()
        {
            DateCreated = DateTime.Now;
        }

        public static bool IsCreationContext { get; set; } = true; // Default to true for creation context

    }
}


