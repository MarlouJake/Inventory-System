using InventorySystem.Models.DataEntities;

namespace InventorySystem.Models.Services
{
    public class AddItemModel
    {
        public IEnumerable<User>? Users { get; set; }
        public IEnumerable<Item>? Items_ { get; set; }
    }
}
