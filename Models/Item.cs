using InventorySystem.Attributes;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace InventorySystem.Models
{
    public class Item
    {
        [DisplayName("Item ID")]
        [Key]
        public int ItemId { get; set; }

        [Required(ErrorMessage = "Item code is required!")]
        [MinLength(3, ErrorMessage = "Item code must be atleast 3 characters.")]
        [NoSpaces(ErrorMessage = "Item code must not contain spaces.")]
        [DisplayName("Item Code")]
        public string? ItemCode { get; set; }

        [Required(ErrorMessage = "Item name is required!")]
        [NoSpaces(ErrorMessage = "Item name must not contain spaces.")]
        [MinLength(3, ErrorMessage = "Item name must be atleast 3 characters.")]
        [DisplayName("Item Name")]
        public string? ItemName { get; set; }


        [DisplayName("Description")]
        public string? ItemDescription { get; set; }

        [DisplayName("Status")]
        [Required(ErrorMessage = "Status is required")]
        public string? Status { get; set; }

        [DisplayName("Additional Information")]
        public string? AdditionalInfo { get; set; }

        [DisplayFormat(DataFormatString = "{0:MMMM dd, yyyy hh:mm:ss tt}", ApplyFormatInEditMode = true)]
        [DisplayName("Date Added")]
        public DateTime ItemDateAdded { get; set; }

        [DisplayFormat(DataFormatString = "{0:MMMM dd, yyyy hh:mm:ss tt}", ApplyFormatInEditMode = true)]
        [DisplayName("Date Modified")]
        public DateTime ItemDateUpdated { get; set; }

        [Required(ErrorMessage = "Firmware Update is required")]
        [DisplayName("Firmware Updated")]
        public string? FirmwareUpdated { get; set; }

        [DisplayName("User ID")]
        public int? UserId { get; set; }
        public Item()
        {
            ItemDateAdded = DateTime.Now;
            ItemDateUpdated = DateTime.Now;
        }
    }
}
