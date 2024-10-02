using InventorySystem.Utilities;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace InventorySystem.Models.Identities
{
    public class Role
    {
        [Key]
        [DisplayName(DisplayNames.RoleId)]
        public int RoleId { get; set; }

        [DisplayName(DisplayNames.Name)]
        public string? Name { get; set; }

        [DisplayName(DisplayNames.Role)]
        public string? RoleDisplayName { get; set; }

        [DisplayName(DisplayNames.UserRole)]
        public ICollection<UserRole>? UserRoles { get; set; }
    }
}
