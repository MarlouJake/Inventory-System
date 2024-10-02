using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        [DisplayFormat(DataFormatString = "{0:MMM. dd, yyyy hh:mm:ss tt}", ApplyFormatInEditMode = true)]
        public DateTime Timestamp { get; set; }
        [ForeignKey("UserId")]
        public int? UserId { get; set; }

        public virtual Item? Items { get; set; }
        public virtual User? Users { get; set; }
    }
}
