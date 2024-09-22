using InventorySystem.Data;
using InventorySystem.Models.DataEntities;
using InventorySystem.Utilities;
using InventorySystem.Utilities.Api;
using InventorySystem.Utilities.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System.Security.Claims;

namespace InventorySystem.Controllers.api
{
    [Route("api/u/services/")]
    [ApiController]
    public class ServicesApi(ApplicationDbContext context) : ControllerBase
    {
        private readonly ApplicationDbContext _context = context;

        [HttpPost("create/user")]
        public async Task<IActionResult> CreateNewAccount([FromBody] User model)
        {
            var newUser = new User
            {
                Username = model.Username,
                Email = model.Email,
                Password = HashHelper.HashPassword(model.Password!),
            };

            var seedrole = new SeedUserRole(_context);
            var (isSuccess, message, statuscode) = await seedrole.AddUserRole(newUser, "User");

            if (isSuccess)
            {
                var redirectTo = "home/login";
                var response = ApiResponseUtils.SuccessResponse(null!, message, redirectTo);
                return StatusCode(statuscode, response);
            }
            else
            {
                var response = ApiResponseUtils.CustomResponse(isSuccess, message, null);
                return StatusCode(statuscode, response);
            }
        }

        [HttpPost("add")]
        public async Task<IActionResult> AppendItem([FromBody][Bind("ItemId,ItemCode,ItemName,ItemDescription,Status,ItemDateAdded,ItemDateUpdated,FirmwareUpdated,UserId")] Item model)
        {
            try
            {
                var url = Url.Action("AppendItem", "ServicesApi");
                Messages.PrintUrl(url);
                Console.WriteLine($"Received data: {JsonConvert.SerializeObject(model)}");

                if (model == null)
                {
                    var message = $"Data is {null}";
                    var response = ApiResponseUtils.CustomResponse(false, message, model);
                    Console.Error.WriteLine(message);
                    return StatusCode(StatusCodes.Status404NotFound, response);
                }

                var existingCode = await _context.Items.AnyAsync(i => i.ItemCode == model.ItemCode);

                if (existingCode)
                {
                    var message = "Item code already in use";
                    var response = ApiResponseUtils.CustomResponse(false, message, model);
                    Console.Error.WriteLine(message);
                    return StatusCode(StatusCodes.Status409Conflict, response);
                }

                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

                if (userIdClaim != null && int.TryParse(userIdClaim, out var userId))
                {
                    model.UserId = userId;

                    _context.Items.Add(model);
                    await _context.SaveChangesAsync();
                    var items = await _context.Items
                        .Where(i => i.UserId == userId)
                        .ToListAsync();

                    var redirectTo = "/dashboard/item-view";
                    var message = "Item added successfully";
                    Console.WriteLine(message);
                    Console.WriteLine("URL: {0}", redirectTo);
                    var response = ApiResponseUtils.SuccessResponse(null!, message, redirectTo);
                    return StatusCode(StatusCodes.Status201Created, response);
                }
                else
                {
                    var message = "User ID not doesn't exist";
                    var response = ApiResponseUtils.CustomResponse(false, message, model);
                    Console.Error.WriteLine(message);
                    return StatusCode(StatusCodes.Status404NotFound, response);

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

                var response = ApiResponseUtils.CustomResponse(false, errorMessage, null);
                return StatusCode(StatusCodes.Status500InternalServerError, response);
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

                var response = ApiResponseUtils.CustomResponse(false, errorMessage, null);
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        [HttpPut("modify/{id?}")]
        public async Task<IActionResult> ModifyItem([FromBody][Bind("ItemId,ItemCode,ItemName,ItemDescription,Status,FirmwareUpdated,ItemDateUpdated,UserId")] Item model, int id)
        {
            string message = "";
            int Status404 = StatusCodes.Status404NotFound;
            int Status500 = StatusCodes.Status500InternalServerError;
            id = model.ItemId;
            var url = Url.Action("ModifyItem", "ServicesApi");
            Messages.PrintUrl(url);
            Console.WriteLine($"Received data: {JsonConvert.SerializeObject(model)}");
            Console.WriteLine("id: ", id);
            Console.WriteLine("input id: {0}", model.ItemId);
            try
            {
                if (model == null)
                {
                    message = $"Data is {null}";
                    var response = ApiResponseUtils.CustomResponse(false, message, model);
                    Console.Error.WriteLine(message);
                    return StatusCode(Status404, response);
                }

                var existingItem = await _context.Items.FindAsync(id);
                if (existingItem == null)
                {
                    message = $"Item not found: {existingItem}, {id}";
                    var response = ApiResponseUtils.CustomResponse(false, message, model);
                    Console.Error.WriteLine(message);
                    return StatusCode(Status404, response);
                }

                // Detach the existing entity to avoid tracking conflicts
                _context.Entry(existingItem).State = EntityState.Detached;

                // Check if the new ItemCode is different from the existing ItemCode
                if (existingItem.ItemCode != model.ItemCode)
                {
                    // Check if the new ItemCode already exists in the database
                    var existingCode = await _context.Items.AnyAsync(i => i.ItemCode == model.ItemCode && i.ItemId != model.ItemId);

                    if (existingCode)
                    {
                        message = "Item code already in use";
                        var response = ApiResponseUtils.CustomResponse(false, message, model);
                        Console.Error.WriteLine(message);
                        return StatusCode(StatusCodes.Status409Conflict, response);
                    }
                }

                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

                if (userIdClaim != null && int.TryParse(userIdClaim, out var userId))
                {
                    model.UserId = userId;

                    model.ItemDateAdded = existingItem.ItemDateAdded;

                    _context.Items.Update(model);
                    await _context.SaveChangesAsync();

                    var items = await _context.Items
                        .Where(i => i.UserId == userId)
                        .ToListAsync();

                    var redirectTo = "/dashboard/item-view";
                    message = "Item updated successfully";
                    Console.WriteLine(message);
                    Console.WriteLine("URL: {0}", redirectTo);
                    var response = ApiResponseUtils.SuccessResponse(null!, message, redirectTo);
                    return StatusCode(StatusCodes.Status200OK, response);
                }
                else
                {
                    message = "User ID not doesn't exist";
                    var response = ApiResponseUtils.CustomResponse(false, message, model);
                    Console.Error.WriteLine(message);
                    return StatusCode(StatusCodes.Status404NotFound, response);

                }

            }
            catch (MySqlException sqlEx)
            {
                message = "An error occurred while connecting to the MySQL database.";

                #region --Console Logger--
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Error.WriteLine($"\nMySQL Exception Caught: {sqlEx}\n");
                Console.ResetColor();
                #endregion

                var response = ApiResponseUtils.CustomResponse(false, message, null);
                return StatusCode(Status500, response);
            }
            catch (Exception ex)
            {
                message = "An unknown error occurred.";

                #region --Console Logger--
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Error.WriteLine($"\nException Caught: {ex}\n");
                Console.ResetColor();
                #endregion

                var response = ApiResponseUtils.CustomResponse(false, message, null);
                return StatusCode(Status500, response);
            }
        }


        [HttpDelete("remove-confirm/{id}")]
        public async Task<IActionResult> RemoveItem(int id)
        {
            string message = "";
            int Status404 = StatusCodes.Status404NotFound;
            int Status500 = StatusCodes.Status500InternalServerError;

            try
            {
                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                Console.WriteLine($"User ID Claim: {userIdClaim}");

                var item = await _context.Items.FindAsync(id);
                if (item == null)
                {
                    message = "Item doesn't exist";
                    var response = ApiResponseUtils.CustomResponse(false, message, null);
                    return await Task.FromResult(StatusCode(Status404, response));
                }


                if (userIdClaim != null && int.TryParse(userIdClaim, out var userId))
                {
                    _context.Items.Remove(item);
                    await _context.SaveChangesAsync();
                    var items = await _context.Items
                            .Where(i => i.UserId == userId)
                            .ToListAsync();

                    var redirectTo = "/dashboard/item-view";
                    message = "Item deleted successfully";
                    Console.WriteLine(message);
                    Console.WriteLine("URL: {0}", redirectTo);
                    var response = ApiResponseUtils.SuccessResponse(null!, message, redirectTo);
                    return StatusCode(StatusCodes.Status200OK, response);
                }
                else
                {
                    message = "User ID not doesn't exist";
                    var response = ApiResponseUtils.CustomResponse(false, message, null);
                    Console.Error.WriteLine(message);
                    return StatusCode(Status404, response);
                }
            }
            catch (MySqlException sqlEx)
            {
                message = "An error occurred while connecting to the MySQL database.";

                #region --Console Logger--
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Error.WriteLine($"\nMySQL Exception Caught: {sqlEx}\n");
                Console.ResetColor();
                #endregion

                var response = ApiResponseUtils.CustomResponse(false, message, null);
                return StatusCode(Status500, response);
            }
            catch (Exception ex)
            {
                message = "An unknown error occurred.";

                #region --Console Logger--
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Error.WriteLine($"\nException Caught: {ex}\n");
                Console.ResetColor();
                #endregion

                var response = ApiResponseUtils.CustomResponse(false, message, null);
                return StatusCode(Status500, response);
            }


        }
    }


}

