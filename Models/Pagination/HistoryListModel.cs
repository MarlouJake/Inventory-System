using InventorySystem.Models.DataEntities;

namespace InventorySystem.Models.Pagination
{
    public class HistoryListModel
    {
        public IEnumerable<CreateHistory>? History { get; set; }
        public int CurrentPage { get; set; }   
        public int TotalPages { get; set; } 
    }
}
