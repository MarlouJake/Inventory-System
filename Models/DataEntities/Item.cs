using InventorySystem.Attributes;
using InventorySystem.Utilities;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace InventorySystem.Models.DataEntities
{
    public class Item
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


        [DisplayName("Status")]
        [DataType(DataType.Text)]
        public string? Status { get; set; }

        [DisplayName("Board Status")]
        [DataType(DataType.Text)]
        public string? FirmwareUpdated { get; set; }


        [DisplayName("Category")]
        [DataType(DataType.Text)]
        public string? Category { get; set; }

        [DisplayFormat(DataFormatString = "{0:MMM. dd, yyyy hh:mm:ss tt}", ApplyFormatInEditMode = true)]

        [DisplayName("Date Added")]
        [DataType(DataType.DateTime)]
        public DateTime ItemDateAdded { get; set; }

        [DisplayFormat(DataFormatString = "{0:MMM. dd, yyyy hh:mm:ss tt}", ApplyFormatInEditMode = true)]

        [DisplayName("Last Modified")]
        [DataType(DataType.DateTime)]
        public DateTime ItemDateUpdated { get; set; }

        [DisplayName(DisplayNames.UserId)]
        public int? UserId { get; set; }

        public Item()
        {
            ItemDateAdded = DateTime.Now;
            ItemDateUpdated = DateTime.Now;
        }

    }
}
