using InventorySystem.Attributes;
using InventorySystem.Utilities;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace InventorySystem.Models.DataEntities
{
    public class Admin
    {
        [Key]
        [DisplayName(DisplayNames.AdminId)]
        public int AdminId { get; set; }

        [DisplayName(DisplayNames.Username)]
        [ValidateField]
        [CharacterLength(3, 30)]
        [DataType(DataType.Text)]
        public string? Username { get; set; }

        [ValidateField]
        [NoSpaces]
        [CharacterLength(8, 128)]
        [PasswordPropertyText]
        [DisplayName(DisplayNames.Password)]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = Messages.FomatEmail)]
        [ValidateField]
        [NoSpaces]
        [DisplayName(DisplayNames.Email)]
        public string? Email { get; set; }

        [DisplayName(DisplayNames.DateAdded)]
        [DisplayFormat(DataFormatString = "{0:MMMM dd, yyyy hh:mm:ss tt}", ApplyFormatInEditMode = true)]
        [DataType(DataType.DateTime)]
        public DateTime DateCreated { get; set; }

        public Admin()
        {
            DateCreated = DateTime.Now;
        }
    }
}
