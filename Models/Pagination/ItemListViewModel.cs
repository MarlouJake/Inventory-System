using InventorySystem.Models.DataEntities;
using InventorySystem.Utilities;
using System.ComponentModel;

namespace InventorySystem.Models.Pagination
{
    public class ItemListViewModel
    {
        public IEnumerable<Item>? Items { get; set; }
        [DisplayName(DisplayNames.CurrentPage)]
        public int CurrentPage { get; set; }
        [DisplayName(DisplayNames.TotalPage)]
        public int TotalPages { get; set; }
        public string? Category { get; set; }
    }

}
