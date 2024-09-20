using InventorySystem.Models.Identities;
using System.ComponentModel.DataAnnotations;

namespace InventorySystem.Models.DataEntities
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public string? Description { get; set; }
        public ICollection<ItemCategory>? ItemCategories { get; set; }
    }
}
