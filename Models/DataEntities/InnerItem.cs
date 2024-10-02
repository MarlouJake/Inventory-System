using InventorySystem.Attributes;
using InventorySystem.Utilities;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventorySystem.Models.DataEntities
{
    public class InnerItem
    {
        [DisplayName(DisplayNames.InnerId)]
        [Key]
        public int InnerItemId { get; set; }

        [ForeignKey(DisplayNames.ItemId)]
        [DisplayName(DisplayNames.ParentId)]
        public int ItemParentId { get; set; }

        [DisplayName(DisplayNames.ItemCode)]
        [ValidateField]
        [NoSpaces]
        [CharacterLength(3, 50)]
        public string? ItemCode { get; set; }

        [ValidateField]
        [CharacterLength(3, 20)]
        [DataType(DataType.Text)]
        [DisplayName(DisplayNames.ItemName)]
        public string? ItemName { get; set; }

        [ValidateField]
        [NoSpaces]
        [DisplayName(DisplayNames.Quantity)]
        [CustomRange(0, 99)]
        public int ItemQuantity { get; set; }

        [ValidateField]
        [NoSpaces]
        [DisplayName(DisplayNames.Description)]
        [CustomRange(0, 5000)]
        [DataType(DataType.MultilineText)]
        public string? ItemDescription { get; set; }

        [DisplayName(DisplayNames.DateAdded)]
        [DataType(DataType.DateTime)]
        public DateTime ItemDateAdded { get; set; }

        [DisplayName(DisplayNames.DateModified)]
        [DataType(DataType.DateTime)]
        public DateTime ItemDateModified { get; set; }
    }
}
