using InventorySystem.Attributes;
using InventorySystem.Utilities;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace InventorySystem.Models.Accounts
{
    public class RegisterModel
    {
        [DisplayName(DisplayNames.UserId)]
        [Key]
        public int UserId { get; set; }

        [DisplayName(DisplayNames.Username)]
        [ValidateField]
        [NoSpaces]
        [CharacterLength(3, 64)]
        [DataType(DataType.Text)]
        public string? Username { get; set; }

        [DisplayName(DisplayNames.Email)]
        [ValidateField]
        [NoSpaces]
        [CharacterLength(3, 64)]
        [EmailAddress(ErrorMessage = Messages.FomatEmail)]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }

        [DisplayName(DisplayNames.Password)]
        [NoSpaces]
        [ValidatePassword(3, 128)]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [DisplayName("Confirm Password")]
        [NoSpaces]
        [ValidatePassword(3, 128)]
        [PasswordMatch]
        [DataType(DataType.Password)]
        public string? ConfirmPassword { get; set; }
    }
}