using InventorySystem.Attributes;
using InventorySystem.Utilities;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace InventorySystem.Models.Services
{
    public class DeleteModel
    {
        [DisplayName(DisplayNames.ItemId)]
        [Key]
        public int ItemId { get; }

        [DisplayName(DisplayNames.ItemCode)]
        public string? ItemCode { get; }

        [DisplayName(DisplayNames.ItemName)]
        public string? ItemName { get; }


        [DisplayName(DisplayNames.Description)]
        public string? ItemDescription { get; }

        [DisplayName(DisplayNames.ItemStatus)]
        public string? Status { get; }

        [ValidateField]
        [DisplayName(DisplayNames.FirmwareStatus)]
        [DataType(DataType.Text)]
        [ValidateValue("firmwarestatus")]
        public string? FirmwareUpdated { get; }

        [DisplayName(DisplayNames.UserId)]
        public int? UserId { get; }

    }
}
