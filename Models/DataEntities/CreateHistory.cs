using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace InventorySystem.Models.DataEntities
{
    public class CreateHistory
    {
        [Key]
        [DisplayName("#")]
        public int? HistoryId { get; set; }
        [ForeignKey("ItemId")]
        public int? ItemId { get; set; }
        [DisplayName("Code")]
        public string? ItemCode { get; set; }
        [DisplayName("Name")]
        public string? ItemName { get; set; }
        public string? Category { get; set; }
        public string? Description { get; set; }

        [DisplayFormat(DataFormatString = "{0:MMM. dd, yyyy hh:mm:ss tt}", ApplyFormatInEditMode = true)]
        public DateTime DateAdded { get; set; }
        [DisplayFormat(DataFormatString = "{0:MMM. dd, yyyy hh:mm:ss tt}", ApplyFormatInEditMode = true)]
        public DateTime? DateUpdated { get; set; }
        public bool IsModified { get; set; }
        public bool IsBorrowed { get; set; }
        public bool IsReturned { get; set; }
        public bool IsRemoved { get; set; }
        public bool HistoryRemoved { get; set; }

        [DisplayName("Added")]
        public string? RelativeTimeStamp { get; set; }

        [DisplayName]
        public string? Status { get; set; } 
        public int? UserId { get; set; }
        public string? Username { get; set; }
    }
}
