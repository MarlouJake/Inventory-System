using InventorySystem.Models.DataEntities;

namespace InventorySystem.Models.Pagination
{
    public class ItemCardModel
    {
        public Item? Items { get; set; }
        public User? Users { get; set; }
    }
}
