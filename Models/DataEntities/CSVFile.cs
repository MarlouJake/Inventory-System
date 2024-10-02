using System.ComponentModel.DataAnnotations;

namespace InventorySystem.Models.DataEntities
{
    public class CSVFile
    {

        public string? ItemCode { get; set; }
        public string? ItemName { get; set; }
        public string? ItemDescription { get; set; }
        public string? Status { get; set; }
        public string? FirmwareUpdated { get; set; }
        public string? Category { get; set; }
        [DisplayFormat(DataFormatString = "{0:MMM. dd, yyyy hh:mm:ss tt}", ApplyFormatInEditMode = true)]
        public DateTime ItemDateAdded { get; set; }
        [DisplayFormat(DataFormatString = "{0:MMM. dd, yyyy hh:mm:ss tt}", ApplyFormatInEditMode = true)]
        public DateTime ItemDateUpdated { get; set; }
        public int? UserId { get; set; }

        public CSVFile()
        {
            ItemDateAdded = DateTime.Now;
            ItemDateUpdated = DateTime.Now;
        }
    }
}
