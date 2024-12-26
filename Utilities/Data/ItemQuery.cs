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
        private int pageSize = 24;

        /// <summary>
        /// Gets the number of items per page.
        /// </summary>
        /// <returns>An integer representing the page size.</returns>
        public int GetPageSize()
        {
            return pageSize; // Return the page size
        }

        public int SetPageSize(int size)
        {
            pageSize = size;
            return pageSize;
        }



        public async Task<ItemListViewModel> Pagination(int? id, int page, string category, string code, int? pagesize = null)
        {
            return await PaginateItems(id, page, pagesize, category, code!);
        }

        public async Task<List<Item>> GetItemListAsync(int? id)
        {
            return await FilterByUserIdAsync(id).ToListAsync();
        }

        public async Task<Item?> GetItemAsync(int id)
        {
            return await GetItemByItemIdAsync(id);
        }

        public async Task<Item?> FindItemAsync(int? id)
        {
            return await FindItemByIdAsync(id);
        }

        public async Task<Item?> FindItemUUIDAsync(Guid id)
        {
            return await FindItemByUUIDAsync(id);
        }

        public async Task<List<Item>> FindItemsAsync(string[] ids)
        {
            return await FindItemByArrayofIdsAsync(ids);
        }

        public async Task<int> CountTotalItems(int? id, string? category = null)
        {
            return await CountTotalItemById(id, category);
        }

        private async Task<ItemListViewModel> PaginateItems(int? id, int page, int? pagesize, string category, string code, bool deleted = false)
        {
            _checkInputs.CheckPage(page);

            var fetchedItems = FilterByUserIdAsync(id, deleted);
            var searchResult = await SearchResult(fetchedItems, id, code);
            var isSearch = String.IsNullOrEmpty(code) ? fetchedItems : searchResult;
            var items = CategoryBase(id, category, isSearch);
            var totalItems = await CountItems(await items);
            int totalPages = await CountTotalPages(totalItems);
            var paginateItems = await PaginateItemsAsync(page, pagesize, await items);
            var paginatedItems = await PaginatedItemsAsync(page, category, totalPages, paginateItems.AsQueryable());
            return paginatedItems;
        }

        private static async Task<IQueryable<Item>> SearchResult(IQueryable<Item> items, int? id, string code)
        {
            //var fetchedItems = FilterByUserIdAsync(id);
            var filteredItem = await FilterByCodeAndUserId(id, code, items);
            return filteredItem;
        }

        private async Task<IQueryable<Item>> CategoryBase(int? id, string category, IQueryable<Item> items)
        {
            _checkInputs.CheckId(id);



            if (category != "All")
            {
                items = items.Where(i => i.Category == category && i.UserId == id);
            }

            return await Task.FromResult(items);
        }



        private async Task<List<Item>> PaginateItemsAsync(int page, int? pagesize, IQueryable<Item> model)
        {
            int pageSizeResult = pagesize != null ? (int)pagesize : pageSize;
            var items = await model
                    .OrderBy(i => i.ItemName)
                    .Skip(((page - 1) * pageSizeResult))
                    .Take(pageSizeResult)
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

        private async Task<int> CountTotalItemById(int? id, string? category = null)
        {
            var items = FilterByUserIdAsync(id);
            var countByCategory = await CategoryBase(id, category!, items);
            var filteredItems = category != null ? countByCategory : items;
            var totalItem = await CountItems(filteredItems);
            return totalItem;
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


        private static async Task<IQueryable<Item>> FilterByCodeAndUserId(int? id, string code, IQueryable<Item> items, bool deleted = false)
        {
            IQueryable<Item> filteredItem = items;

            if (!string.IsNullOrEmpty(code))
            {
                filteredItem = items.Where(i => i.ItemCode!.StartsWith(code) && i.UserId == id && i.IsDeleted == deleted);
            }
            return await Task.FromResult(filteredItem);
        }



        private IQueryable<Item> FilterByUserIdAsync(int? id, bool deleted = false)
        {
            return _context.Items.Where(i => i.UserId == id && i.IsDeleted == deleted);
        }


        private async Task<Item?> GetItemByUserIdAsync(int? id, bool deleted = false)
        {
            return await _context.Items.FirstOrDefaultAsync(i => i.UserId == id && i.IsDeleted == deleted);
        }

        private async Task<Item?> GetItemByItemIdAsync(int? id, bool deleted = false)
        {
            return await _context.Items.FirstOrDefaultAsync(i => i.ItemId == id && i.IsDeleted == deleted);
        }

        private async Task<Item?> FindItemByIdAsync(int? id)
        {
            return await _context.Items.FindAsync(id);
        }

        private async Task<Item?> FindItemByUUIDAsync(Guid id)
        {
            return await _context.Items.FirstOrDefaultAsync(i => i.UniqueId == id);
        }

        private async Task<List<Item>> FindItemByArrayofIdsAsync(string[] ids, bool deleted = false)
        {
            List<Guid> uniqueIds = ids.Where(id => Guid.TryParse(id, out _))
                .Select(Guid.Parse).ToList();

            return await _context.Items.Where(i => uniqueIds.Contains(i.UniqueId) && i.IsDeleted == deleted).ToListAsync();
        }

    }
}
