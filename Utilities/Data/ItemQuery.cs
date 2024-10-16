using InventorySystem.Data;
using InventorySystem.Models.DataEntities;
using InventorySystem.Models.Pagination;
using Microsoft.EntityFrameworkCore;

namespace InventorySystem.Utilities.Data
{
    /// <summary>
    /// The ItemQuery class provides methods to interact with inventory items.
    /// It facilitates retrieving and paginating items based on a user's ID.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the ItemQuery class.
    /// </remarks>
    /// <param name="context">The database context for accessing items.</param>
    /// <param name="checkInputs">An instance to validate input parameters.</param>
    public class ItemQuery(ApplicationDbContext context, CheckInputs checkInputs)
    {
        private readonly ApplicationDbContext _context = context;
        private readonly CheckInputs _checkInputs = checkInputs;
        private readonly int pageSize = 24; // Number of items per page



        /// <summary>
        /// Gets the number of items per page.
        /// </summary>
        /// <returns>An integer representing the page size.</returns>
        public int GetPageSize()
        {
            return pageSize; // Return the page size
        }



        public async Task<ItemListViewModel> Pagination(int? id, int page, string category, string action, string code = null!)
        {

            return await PaginateItems(id, page, category, code);
        }



        public async Task<Item?> FindItemAsync(int? id)
        {
            return await FindItemByIdAsync(id);
        }



        private async Task<ItemListViewModel> PaginateItems(int? id, int page, string category, string code = null!)
        {
            var fetchedItems = FilterByUserIdAsync(id);
            var isSearch = code != null ? await SearchResult(id, page, category, code) : fetchedItems;
            var items = CategoryBase(id, page, category, isSearch);
            var totalItems = await CountItems(await items);
            int totalPages = await CountTotalPages(totalItems);
            var paginateItems = await PaginateItemsAsync(page, await items);
            var paginatedItems = await PaginatedItemsAsync(page, category, totalPages, paginateItems.AsQueryable());
            return paginatedItems;
        }

        private async Task<IQueryable<Item>> SearchResult(int? id, int page, string category, string code)
        {
            var fetchedItems = FilterByUserIdAsync(id);
            var filteredItem = await FilterByCodeAndUserId(id, code, fetchedItems);
            /*var items = CategoryBase(id, page, category, filteredItem);
            var totalItems = await CountItems(filteredItem);
            var totalPages = await CountTotalPages(totalItems);
            var paginateItems = await PaginateItemsAsync(page, filteredItem);
            var paginatedItems = await PaginatedItemsAsync(page, category, totalPages, paginateItems.AsQueryable());
            return paginatedItems;*/
            //var paginateItems = await PaginateItems(id, page, category, code);
            return filteredItem;
        }

        private async Task<IQueryable<Item>> CategoryBase(int? id, int page, string category, IQueryable<Item> items)
        {
            _checkInputs.CheckId(id);
            _checkInputs.CheckPage(page);


            if (category != "All")
            {
                items = items.Where(i => i.Category == category && i.UserId == id);
            }

            return await Task.FromResult(items.AsQueryable());
        }



        private async Task<List<Item>> PaginateItemsAsync(int page, IQueryable<Item> model)
        {
            var items = await model
                    .OrderBy(i => i.ItemName)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

            return items;
        }



        private static async Task<ItemListViewModel> PaginatedItemsAsync(int page, string category, int total, IQueryable<Item> items)
        {

            var model = new ItemListViewModel
            {
                Items = items,
                CurrentPage = page,
                TotalPages = total,
                Category = category
            };

            return await Task.FromResult(model);
        }



        private static async Task<int> CountItems(IQueryable<Item> items)
        {
            var count = await items.CountAsync();
            return count;
        }



        private async Task<int> CountTotalPages(int totalItems)
        {
            return await Task.FromResult((int)Math.Ceiling(totalItems / (double)pageSize));
        }


        private static async Task<IQueryable<Item>> FilterByCodeAndUserId(int? id, string code, IQueryable<Item> items)
        {
            IQueryable<Item> filteredItem = items;

            if (!string.IsNullOrEmpty(code))
            {
                filteredItem = items.Where(i => i.ItemCode!.StartsWith(code) && i.UserId == id);
            }

            return await Task.FromResult(filteredItem);
        }



        private IQueryable<Item> FilterByUserIdAsync(int? id)
        {
            return _context.Items.Where(i => i.UserId == id);
        }




        private async Task<Item?> FindItemByIdAsync(int? id)
        {
            return await _context.Items.FindAsync(id);
        }



    }
}
