using System.ComponentModel.DataAnnotations;

namespace InventorySystem.Models.Identities
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        public string? Name { get; set; }
        public string? CategoryDisplayName { get; set; }
        public ICollection<ItemCategory>? ItemCategories { get; set; }
    }
}
