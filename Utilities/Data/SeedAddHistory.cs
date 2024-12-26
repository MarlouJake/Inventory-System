using Azure;
using InventorySystem.Data;
using InventorySystem.Models.DataEntities;
using InventorySystem.Models.Identities;
using InventorySystem.Models.Responses;
using InventorySystem.Utilities.Api;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using System.Security.Claims;

namespace InventorySystem.Utilities.Data
{
    public class SeedAddHistory
    {
        private readonly ApplicationDbContext _context;
        private readonly GetClaims _getClaims;
        /// <summary>
        /// Initializes a new instance of the <see cref="SeedAddHistory"/> class.
        /// </summary>
        /// <param name="context">An instance of <see cref="ApplicationDbContext"/> for database operations.</param>
        public SeedAddHistory(ApplicationDbContext context, GetClaims getClaims)
        {
            _context = context;
            _getClaims = getClaims;
        }

        public async Task<(bool, string, int)> AddToCreateHistory(Item recentItem, ClaimsPrincipal claim)
        {
            int Added = StatusCodes.Status200OK;
            int Conflict = StatusCodes.Status409Conflict;
            int InternalServerError = StatusCodes.Status500InternalServerError;

            string message = "";

            try
            {
                var existingItem = await _context.CreateHistories.AnyAsync(i => i.ItemCode == recentItem.ItemCode);

                if (existingItem)
                {
                    message = "Item code is already in use";
                    return (false, message, Conflict);
                }

                var item = new CreateHistory
                {
                    ItemCode = recentItem.ItemCode,
                    ItemName = recentItem.ItemName,
                    Category = recentItem.Category,
                    Description = recentItem.ItemDescription,
                    DateAdded = recentItem.ItemDateAdded,
                    DateUpdated = recentItem.ItemDateUpdated,
                    IsModified = recentItem.IsModified,
                    IsBorrowed = recentItem.IsBorrowed,
                    IsReturned = recentItem.IsReturned,
                    IsRemoved = false,
                    HistoryRemoved = false,
                    Status = "New",
                    Id = recentItem.ItemId,
                    UniqueID = recentItem.UniqueId,
                    UserId = _getClaims.GetIdClaim(claim),
                    Username = _getClaims.GetUsernameClaim(claim)
                };

                _context.CreateHistories.Add(item);
                await _context.SaveChangesAsync();

                return (true, message, Added);

            }
            catch (MySqlException sqlEx)
            {
                message = "An error occurred while connecting to the MySQL database.";
                // Log the SQL exception
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Error.WriteLine($"\nMySQL Exception Caught: {sqlEx}\n");
                Console.ResetColor();

                return (false, message, InternalServerError);
            }
            catch (Exception ex)
            {
                message = "An unknown error occurred.";
                // Log any other exception
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Error.WriteLine($"\nException Caught: {ex}\n");
                Console.ResetColor();

                return (false, message, InternalServerError);
            }
        }

        public async Task<(bool, string, int)> UpdateCreateHistory(Item modifiedItem, ClaimsPrincipal claim, string? action = null, bool borrowed = false, bool returned = false)
        {
            int Updated = StatusCodes.Status204NoContent;
            int Conflict = StatusCodes.Status409Conflict;
            int InternalServerError = StatusCodes.Status500InternalServerError;
            bool isRemoved = false;
            string status = "";
            string message = "";

            try
            {
                var existingItem = await _context.Items.FindAsync(modifiedItem.ItemId);
                if (existingItem == null)
                {
                    isRemoved = true;
                }

                if (existingItem!.Status != modifiedItem.Status)
                {
                    var existingStatus = await _context.CreateHistories.AnyAsync(i => i.Status == modifiedItem.Status && i.HistoryId == modifiedItem.ItemId);

                    if (existingStatus)
                    {
                        message = "Status already in use";
                        Console.Error.WriteLine(message);
                        return (false, message, Conflict);
                    }
                }

                status = action switch
                {
                    "update" => "Modified",
                    "borrow" => "Borrowed",
                    "remove" => "Removed",
                    _ => throw new InvalidOperationException($"Action {action} not in range")
                };

                Console.WriteLine("Status {0}", status);
                var existingHistory = await _context.CreateHistories
                        .FirstOrDefaultAsync(h => h.HistoryId == modifiedItem.ItemId);

                existingHistory!.ItemCode = modifiedItem.ItemCode;
                existingHistory.ItemName = modifiedItem.ItemName;
                existingHistory.Category = modifiedItem.Category;
                existingHistory.Description = modifiedItem.ItemDescription;
                existingHistory.DateAdded = modifiedItem.ItemDateAdded;
                existingHistory.DateUpdated = modifiedItem.ItemDateUpdated;
                existingHistory.UserId = modifiedItem.UserId;
                existingHistory.Username = _getClaims.GetUsernameClaim(claim);
                existingHistory.IsModified = true;
                existingHistory.IsBorrowed = borrowed || modifiedItem.IsBorrowed;
                existingHistory.IsReturned = returned || modifiedItem.IsReturned;
                existingHistory.IsRemoved = isRemoved;
                existingHistory.Status = status;
                existingHistory.Id = modifiedItem.ItemId;
              
                await _context.SaveChangesAsync();

                return (true, message, Updated);

            }
            catch (MySqlException sqlEx)
            {
                message = "An error occurred while connecting to the MySQL database.";
                // Log the SQL exception
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Error.WriteLine($"\nMySQL Exception Caught: {sqlEx}\n");
                Console.ResetColor();

                return (false, message, InternalServerError);
            }
            catch (Exception ex)
            {
                message = "An unknown error occurred.";
                // Log any other exception
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Error.WriteLine($"\nException Caught: {ex}\n");
                Console.ResetColor();

                return (false, message, InternalServerError);
            }

        }


        /// <summary>
        /// Sets the `HistoryRemoved` flag to true for a specified item in history.
        /// </summary>
        /// <param name="itemid">The ID of the item for which the history record is to be updated.</param>
        /// <returns>
        /// A tuple where:
        /// - Item1 is a boolean indicating success or failure,
        /// - Item2 is a message describing the operation result,
        /// - Item3 is an HTTP status code reflecting the operation outcome.
        /// </returns>
        public async Task<(bool, string, int)> RemoveItemHistory(int itemid)
        {
            int Updated = StatusCodes.Status204NoContent;
            int InternalServerError = StatusCodes.Status500InternalServerError;
            int NotFound = StatusCodes.Status404NotFound;
            string message = "";

            try
            {
                var existingHistory = await _context.CreateHistories
                        .FirstOrDefaultAsync(h => h.Id == itemid);

                if (existingHistory == null) {
                    message = "Item not found";
                    return (false, message, NotFound);              
                }

                existingHistory!.HistoryRemoved = true;

                message = "Delete Success";

                _context.CreateHistories.Update(existingHistory);
                await _context.SaveChangesAsync();

                return (true, message, Updated);

            }
            catch (MySqlException sqlEx)
            {
                message = "An error occurred while connecting to the MySQL database.";
                // Log the SQL exception
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Error.WriteLine($"\nMySQL Exception Caught: {sqlEx}\n");
                Console.ResetColor();

                return (false, message, InternalServerError);
            }
            catch (Exception ex)
            {
                message = "An unknown error occurred.";
                // Log any other exception
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Error.WriteLine($"\nException Caught: {ex}\n");
                Console.ResetColor();

                return (false, message, InternalServerError);
            }

        }
    }
}
