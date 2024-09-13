using InventorySystem.Attributes;
using InventorySystem.Utilities;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace InventorySystem.Models.Accounts
{
    public class AdminModel
    {
        [DisplayName(DisplayNames.AdminId)]
        public int AdminId { get; set; }

        [ValidateUsernameOrEmail]
        [DisplayName(DisplayNames.Username)]
        [MaxLength(254, ErrorMessage = Messages.MaximumUsernameEmailReached)]
        [JsonPropertyName("username")]
        public string? Username { get; set; }


        [DisplayName(DisplayNames.Password)]
        [MaxLength(128, ErrorMessage = Messages.MaximumPasswordReached)]
        [DataType(DataType.Password)]
        [JsonPropertyName("password")]
        public string? Password { get; set; }
    }
}
