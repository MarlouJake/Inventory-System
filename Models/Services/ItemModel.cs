using InventorySystem.Models.DataEntities;
using InventorySystem.Models.DropdownModels;
namespace InventorySystem.Models.Services
{
    public class ItemModel
    {
        public Item? Items { get; set; }
        public List<DropdownModel>? Dropdown { get; set; }
    }
}
