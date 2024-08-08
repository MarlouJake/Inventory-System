using InventorySystem.Attributes;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventorySystem.Models
{
    public class ItemList
    {
        [DisplayName("Inner Item ID")]
        [Key]
        public int InnerItemId { get; set; }


        [ForeignKey("ItemId")]
        [DisplayName("Parent Item Id")]
        public int ItemParentId { get; set; }

        [DisplayName("Code")]
        public string? ItemCode { get; set; }

        [Required(ErrorMessage = "Item name is required.")]
        [NoSpaces(ErrorMessage = "Item name must not contain spaces.")]
        [MinLength(3, ErrorMessage = "Item name must be atleast 3 characters.")]
        [DisplayName("Item Name")]
        public string? ItemName { get; set; }

        [NoSpaces(ErrorMessage = "Quantity name must not contain spaces.")]
        [DisplayName("Quantity")]
        [Range(0, 99, ErrorMessage = "Invalid input")]
        public int ItemQuantity { get; set; }

        [DisplayName("Description")]
        public string? ItemDescription { get; set; }

        [DisplayName("Date Added")]
        public DateTime ItemDateAdded { get; set; }

        [DisplayName("Date Modified")]
        public DateTime ItemDateModified { get; set; }
    }
}
