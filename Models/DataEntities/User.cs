using InventorySystem.Attributes;
using InventorySystem.Models.Identities;
using InventorySystem.Utilities;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventorySystem.Models.DataEntities
{
    public class User
    {

        [DisplayName(DisplayNames.UserId)]
        [Key]
        public int UserId { get; set; }

        [ForeignKey("AdminId")]
        [DisplayName(DisplayNames.AdminId)]
        public int AdminId { get; set; }

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

        [DisplayName(DisplayNames.DateAdded)]
        [DisplayFormat(DataFormatString = "{0:MMMM dd, yyyy hh:mm:ss tt}", ApplyFormatInEditMode = true)]
        [DataType(DataType.DateTime)]
        public DateTime DateCreated { get; set; }

        // Navigation property to access roles associaed with this user
        [DisplayName(DisplayNames.UserRole)]
        public ICollection<UserRole>? UserRoles { get; set; }

        public User()
        {
            DateCreated = DateTime.Now;
        }

    }
}


