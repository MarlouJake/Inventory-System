using InventorySystem.Data;
using InventorySystem.Models.DataEntities;
using InventorySystem.Models.Identities;
using InventorySystem.Models.Pagination;
using Microsoft.EntityFrameworkCore;

namespace InventorySystem.Utilities.Data
{
    /// <summary>
    /// The <see cref="HistoryQuery"/> class provides methods for retrieving, formatting, and paginating 
    /// <see cref="CreateHistory"/> records from the database using <see cref="ApplicationDbContext"/>.
    /// </summary>
    public class HistoryQuery
    {
        private readonly ApplicationDbContext _context;
        private readonly int pageSize = 5;

        /// <summary>
        /// Initializes a new instance of the <see cref="HistoryQuery"/> class.
        /// </summary>
        /// <param name="context">The database context used for accessing <see cref="CreateHistory"/> records.</param>
        public HistoryQuery(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves <see cref="CreateHistory"/> items based on the specified format.
        /// </summary>
        /// <param name="format">The format for retrieving items. Options include:
        /// <list type="bullet">
        /// <item>"format" - Retrieves all items with a relative time format applied.</item>
        /// <item>"paginatedformat" - Retrieves paginated items with a relative time format applied.</item>
        /// <item>"paginate" - Retrieves paginated items without applying any format.</item>
        /// <item>"default" - Retrieves all items without applying any format.</item>
        /// </list>
        /// </param>
        /// <returns>A list of <see cref="CreateHistory"/> items.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the format is invalid.</exception>
        public async Task<HistoryListModel> Pagination(int? id, int page)
        {
            return await PaginateHistory(id, page);
        }

        public async Task<string?> GetTimeStampByItemId(int id)
        {
            return await Task.FromResult(FilterTimeSpanByItemId(id));
        }




        /*
         * 
         * 
         * Retrieving
         * 
         * 
         *          
        */

        /// <summary>
        /// Retrieves all <see cref="CreateHistory"/> items from the database.
        /// </summary>
        /// <returns>A list of all <see cref="CreateHistory"/> items.</returns>
        private async Task<List<CreateHistory>> RetrievAllItems()
        {
            return await _context.CreateHistories.ToListAsync();
        }

        private IQueryable<CreateHistory> FilterByUserIdAsync(int? id)
        {
            return _context.CreateHistories.Where(i => i.UserId == id && i.HistoryRemoved == false);
        }

        private string? FilterTimeSpanByItemId(int id)
        {
            var item = _context.CreateHistories
                .Where(i => i.ItemId == id).FirstOrDefault()!;
            string? timestamp = null;

            if (item.DateAdded.ToString() != null)
            {
                item.RelativeTimeStamp = item.DateAdded.GetRelativeTime();
            }
            else
            {
                Console.WriteLine("DateAdded is null for item ID: " + item.ItemId);
            }

            timestamp = item.RelativeTimeStamp;

            Console.WriteLine($"Timestamp from query: {id} = " + timestamp);
            return timestamp;
        }



        /*
         * 
         * 
         * Changing Format of Timestamps
         * 
         * 
         *          
        */

        private async Task<IQueryable<CreateHistory>> ChangeTimeFormatById(int? id)
        {
            var items = FilterByUserIdAsync(id);
            RelativeTimeList(items);
            return await Task.FromResult(items);
        }


        private static void RelativeTimeList(IQueryable<CreateHistory> items)
        {
            foreach (var item in items)
            {
                if (item.DateAdded.ToString() != null)
                {
                    item.RelativeTimeStamp = item.DateAdded.GetRelativeTime();
                }
                else
                {
                    Console.WriteLine("DateAdded is null for item ID: " + item.ItemId);
                }
            }
        }





        /*
         * 
         * 
         * Counting Items
         * 
         * 
         */
        private async Task<int> CountItemsByUserId(int? id)
        {
            var count = await _context.CreateHistories.Where(i => i.UserId == id).CountAsync();
            return count;
        }

        private static async Task<int> CountItems(IQueryable<CreateHistory> items)
        {
            var count = await items.CountAsync();
            return count;
        }

        private async Task<int> CountTotalPages(int totalItems)
        {
            return await Task.FromResult((int)Math.Ceiling(totalItems / (double)pageSize));
        }





        /*
         * 
         * 
         * Pagination
         * 
         * 
         * 
        */

        private async Task<HistoryListModel> PaginateHistory(int? id, int page)
        {
            var items = await ChangeTimeFormatById(id);
            int totalItems = await CountItemsByUserId(id);
            int totalPages = await CountTotalPages(totalItems);
            var paginateHistory = await PaginateHistoryAsync(page, items);
            var paginatedHistory = await PaginatedHistoryAsync(page, totalPages, paginateHistory.AsQueryable());
            return paginatedHistory;
        }

        /// <summary>
        /// Retrieves paginated <see cref="CreateHistory"/> items.
        /// </summary>
        /// <returns>A paginated list of <see cref="CreateHistory"/> items.</returns>
        private async Task<List<CreateHistory>> PaginateHistoryAsync(int page, IQueryable<CreateHistory> history)
        {
            var items = await history
                    .OrderByDescending(i => i.DateAdded)
                    .Skip(((page - 1) * pageSize))
                    .Take(pageSize)
                    .ToListAsync();
            return items;
        }

        /// <summary>
        /// Generates a paginated list model of <see cref="CreateHistory"/> items.
        /// </summary>
        /// <param name="page">The current page number.</param>
        /// <param name="total">The total number of pages.</param>
        /// <param name="items">The items to be included in the paginated result.</param>
        /// <returns>A <see cref="HistoryListModel"/> containing the paginated <see cref="CreateHistory"/> items.</returns>
        private static async Task<HistoryListModel> PaginatedHistoryAsync(int page, int total, IQueryable<CreateHistory> items)
        {
            var model = new HistoryListModel
            {
                History = items,
                CurrentPage = page,
                TotalPages = total
            };

            return await Task.FromResult(model);
        }
    }
}
