using InventorySystem.Models.DataEntities;
using InventorySystem.Utilities;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace InventorySystem.Models.Identities
{
    public class UserRole
    {
        [Key, Column(Order = 0)]
        [DisplayName(DisplayNames.UserId)]
        public int UserId { get; set; }


        [DisplayName(DisplayNames.Name)]
        public User? User { get; set; }

        [Key, Column(Order = 1)]
        [DisplayName(DisplayNames.RoleId)]
        public int RoleId { get; set; }

        [DisplayName(DisplayNames.UserRole)]
        public Role? Role { get; set; }

    }
}
