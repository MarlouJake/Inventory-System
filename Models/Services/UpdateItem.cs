using InventorySystem.Attributes;
using InventorySystem.Utilities;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace InventorySystem.Models.Services
{
    public class UpdateItem
    {
        [DisplayName(DisplayNames.ItemId)]
        [Key]
        public int ItemId { get; set; }

        [ValidateField]
        [NoSpaces]
        [CharacterLength(3, 20)]
        [DisplayName(DisplayNames.ItemCode)]
        public string? ItemCode { get; set; }

        [ValidateField]
        [CharacterLength(3, 20)]
        [DisplayName(DisplayNames.ItemName)]
        public string? ItemName { get; set; }


        [DisplayName(DisplayNames.Description)]
        [DataType(DataType.MultilineText)]
        [MaxLength(5000, ErrorMessage = "Description must not exceed 5000 characters")]
        public string? ItemDescription { get; set; }

        [DisplayName(DisplayNames.ItemStatus)]
        [DataType(DataType.Text)]
        [ValidateField]
        [ValidateValue("itemstatus")]
        public string? Status { get; set; }


        [ValidateField]
        [DisplayName(DisplayNames.FirmwareStatus)]
        [DataType(DataType.Text)]
        [ValidateValue("firmwarestatus")]
        public string? FirmwareUpdated { get; set; }

        [DisplayFormat(DataFormatString = "{0:MMMM dd, yyyy hh:mm:ss tt}", ApplyFormatInEditMode = true)]
        [DisplayName("Last Modified")]
        [DataType(DataType.DateTime)]
        public DateTime ItemDateUpdated { get; set; }


        [DisplayName(DisplayNames.UserId)]
        public int? UserId { get; set; }

        public UpdateItem()
        {
            ItemDateUpdated = DateTime.Now;
        }

    }
}
