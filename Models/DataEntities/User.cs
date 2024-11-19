using InventorySystem.Attributes;
using InventorySystem.Models.Identities;
using InventorySystem.Utilities;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace InventorySystem.Models.DataEntities
{
    public class User
    {

        [Key]
        [DisplayName(DisplayNames.UserId)]
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

        [DisplayName(DisplayNames.DateAdded)]
        [DisplayFormat(DataFormatString = "{0:MMMM dd, yyyy hh:mm:ss tt}", ApplyFormatInEditMode = true)]
        [DataType(DataType.DateTime)]
        public DateTime DateCreated { get; set; }

        [DisplayName(DisplayNames.DateModified)]
        [DisplayFormat(DataFormatString = "{0:MMMM dd, yyyy hh:mm:ss tt}", ApplyFormatInEditMode = true)]
        [DataType(DataType.DateTime)]
        public DateTime DateModified { get; set; }
        // Navigation property to access roles associaed with this user
        [DisplayName(DisplayNames.UserRole)]
        public ICollection<UserRole>? UserRoles { get; set; }

        // Navigation property to show the history created by the user
        public ICollection<CreateHistory>? CreateHistories { get; set; }
        public ICollection<Item>? Items { get; set; }
        public User()
        {
            DateCreated = DateTime.Now;
            DateModified = DateTime.Now;
        }

        public void Construct(string? username, string? email, string? password)
        {
            Username = username;
            Email = email;
            Password = password;
        }

    }
}


