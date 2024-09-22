using InventorySystem.Models.DataEntities;
using System.ComponentModel.DataAnnotations;

namespace InventorySystem.Models.Identities
{
    public class ItemCategory
    {
        [Key]
        public int ItemCategoryId { get; set; }

        public int ItemId { get; set; }
        public Item? Item { get; set; }

        public int CategoryId { get; set; }
        public Category? Category { get; set; }
    }
}
