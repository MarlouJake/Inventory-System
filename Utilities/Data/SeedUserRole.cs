using InventorySystem.Data;
using InventorySystem.Models.DataEntities;
using InventorySystem.Models.Identities;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;

namespace InventorySystem.Utilities.Data
{
    /// <summary>
    /// The <see cref="SeedUserRole"/> class provides functionality for seeding user roles in the application.
    /// It includes methods for adding a new user along with a specified role to the database.
    /// </summary>
    public class SeedUserRole
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="SeedUserRole"/> class.
        /// </summary>
        /// <param name="context">An instance of <see cref="ApplicationDbContext"/> for database operations.</param>
        public SeedUserRole(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Adds a new user with a specified role to the database.
        /// </summary>
        /// <param name="newUser">The new user to be added.</param>
        /// <param name="roleName">The name of the role to assign to the new user.</param>
        /// <returns>
        /// A tuple containing:
        /// <list type="bullet">
        /// <item>
        /// <term>bool</term>: A value indicating whether the operation was successful.
        /// </item>
        /// <item>
        /// <term>List&lt;string&gt;</term>: A list of messages providing feedback on the operation's outcome.
        /// </item>
        /// <item>
        /// <term>int</term>: The HTTP status code representing the result of the operation.
        /// </item>
        /// </list>
        /// </returns>
        public async Task<(bool, List<string>, int)> AddUserWithRole(User newUser, string roleName)
        {
            int Created = StatusCodes.Status201Created;
            int NotFound = StatusCodes.Status404NotFound;
            int Conflict = StatusCodes.Status409Conflict;
            int InternalServerError = StatusCodes.Status500InternalServerError;

            var messages = new List<string>();

            try
            {
                // Check if the username already exists
                var existingUsername = await _context.Users.AnyAsync(u => u.Username == newUser.Username);
                // Check if the email already exists
                var existingEmail = await _context.Users.AnyAsync(u => u.Email == newUser.Email);

                if (existingUsername)
                {
                    messages.Add("Username is already in use");
                }

                if (existingEmail)
                {
                    messages.Add("Email is already in use");
                }

                if (messages.Count > 0)
                {
                    // Return if there are any validation issues
                    return (false, messages, Conflict);
                }

                // Add the new user to the database
                _context.Users.Add(newUser);
                await _context.SaveChangesAsync();

                // Retrieve the role from the database
                var role = await _context.Roles.FirstOrDefaultAsync(r => r.Name == roleName);
                if (role != null)
                {
                    messages.Add("Registered successfully");
                    // Create a new UserRole entity to associate the user with the role
                    var insertUserRole = new UserRole
                    {
                        UserId = newUser.UserId,
                        RoleId = role.RoleId
                    };

                    // Add the UserRole entity to the database
                    _context.UserRoles.Add(insertUserRole);
                    await _context.SaveChangesAsync();
                    return (true, messages, Created);
                }
                else
                {
                    messages.Add("The specified role doesn't exist");
                    return (false, messages, NotFound);
                }
            }
            catch (MySqlException sqlEx)
            {
                messages.Add("An error occurred while connecting to the MySQL database.");
                // Log the SQL exception
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Error.WriteLine($"\nMySQL Exception Caught: {sqlEx}\n");
                Console.ResetColor();

                return (false, messages, InternalServerError);
            }
            catch (Exception ex)
            {
                messages.Add("An unknown error occurred.");
                // Log any other exception
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Error.WriteLine($"\nException Caught: {ex}\n");
                Console.ResetColor();

                return (false, messages, InternalServerError);
            }
        }
    }
}
