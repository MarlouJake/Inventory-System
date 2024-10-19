using InventorySystem.Data;
using InventorySystem.Models.DataEntities;
using InventorySystem.Models.Identities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;

namespace InventorySystem.Utilities.Data
{
    public class SeedAddHistory
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="SeedAddHistory"/> class.
        /// </summary>
        /// <param name="context">An instance of <see cref="ApplicationDbContext"/> for database operations.</param>
        public SeedAddHistory(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<(bool ,string, int)> AddToCreateHistory(CreateHistory recentItem)
        {
            int Created = StatusCodes.Status201Created;
            int Conflict = StatusCodes.Status409Conflict;
            int InternalServerError = StatusCodes.Status500InternalServerError;


            string message = "";


            try
            {
                var existingItem = await _context.CreateHistories.AnyAsync(i => i.ItemCode ==recentItem.ItemCode);

                if (existingItem)
                {
                    message = "Item code is already in use";
                    return (false, message, Conflict);
                }

                _context.CreateHistories.Add(recentItem);
                await _context.SaveChangesAsync();

                return (true, message, Created); 

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
                message =  "An unknown error occurred." ;
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
