using InventorySystem.Data;
using InventorySystem.Models.DataEntities;
using InventorySystem.Models.Identities;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;

namespace InventorySystem.Utilities.Data
{
    public class SeedUserRole(ApplicationDbContext context)
    {
        private readonly ApplicationDbContext _context = context;
        public async Task<(bool, string, int)> AddUserRole(User newUser, string roleName)
        {
            int Created = StatusCodes.Status201Created;
            int NotFound = StatusCodes.Status404NotFound;
            int Conflict = StatusCodes.Status409Conflict;
            int InternalServerError = StatusCodes.Status500InternalServerError;
            try
            {
                var exisitngUsername = await _context.Users.AnyAsync(u => u.Username == newUser.Username);
                var existingEmail = await _context.Users.AnyAsync(u => u.Email == newUser.Email);

                if (exisitngUsername)
                {
                    return (exisitngUsername, "Username is already in use", Conflict);
                }
                if (existingEmail)
                {
                    return (existingEmail, "Email is already in use", Conflict);
                }

                _context.Users.Add(newUser);
                await _context.SaveChangesAsync();

                var role = await _context.Roles.FirstOrDefaultAsync(r => r.Name == roleName);
                if (role != null)
                {
                    var insertUserRole = new UserRole
                    {
                        UserId = newUser.UserId,
                        RoleId = role.RoleId
                    };

                    _context.UserRoles.Add(insertUserRole);
                    await _context.SaveChangesAsync();
                    return (true, "Account created successfully", Created);
                }
                else
                {
                    return (false, "The specified role doesn't exist", NotFound);
                }
            }
            catch (MySqlException sqlEx)
            {
                var errorMessage = "An error occurred while connecting to the MySQL database.";

                #region --Console Logger--
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Error.WriteLine($"\nMySQL Exception Caught: {sqlEx}\n");
                Console.ResetColor();
                #endregion


                return (false, errorMessage, InternalServerError);
            }
            catch (Exception ex)
            {
                var errorMessage = "An unknown error occurred.";

                #region --Console Logger--
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Error.WriteLine($"\nException Caught: {ex}\n");
                Console.ResetColor();
                #endregion

                return (false, errorMessage, InternalServerError);
            }
        }
    }
}