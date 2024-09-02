namespace InventorySystem.Models
{
    public class ItemListViewModel
    {
        public IEnumerable<Item> Items { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }

}
