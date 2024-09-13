using InventorySystem.Attributes;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace InventorySystem.Models.Services
{
    public class UpdateModel
    {
        [DisplayName("User ID")]
        public int UserId { get; set; }
        [ValidateField]
        [NoSpaces]
        [CharacterLength(3, 20)]
        [DisplayName("Username")]
        [DataType(DataType.Text)]
        public string? Username { get; set; }



        [Required(ErrorMessage = "This field is required!")]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        [DisplayName("Email")]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }


        [NoSpaces]
        [CharacterLength(3, 20)]
        [DisplayName("Password")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }


        [DisplayName("Date Created")]
        [DisplayFormat(DataFormatString = "{0:MMMM dd, yyyy hh:mm:ss tt}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime DateCreated { get; set; }

    }
}
