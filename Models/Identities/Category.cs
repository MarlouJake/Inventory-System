using System.ComponentModel.DataAnnotations;

namespace InventorySystem.Models.Identities
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
