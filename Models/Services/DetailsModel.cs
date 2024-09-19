using InventorySystem.Models.DataEntities;

namespace InventorySystem.Models.Services
{
    public class DetailsModel
    {
        public Item? Items { get; set; }
        public DeleteModel? Delete { get; }
    }
}
