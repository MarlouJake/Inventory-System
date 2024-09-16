using InventorySystem.Data;
using InventorySystem.Models.DataEntities;
using InventorySystem.Utilities;
using InventorySystem.Utilities.Api;
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

        [HttpPost("add-item")]
        public async Task<IActionResult> AppendItem([FromBody][Bind("ItemId,ItemCode,ItemName,ItemDescription,Status,ItemDateAdded,ItemDateUpdated,FirmwareUpdated,UserId")] Item model)
        {
            try
            {
                var url = Url.Action("AppendItem", "ServicesApi");
                Messages.PrintUrl(url);
                Console.WriteLine($"Received data: {JsonConvert.SerializeObject(model)}");

                if (model == null)
                {
                    var message = $"{model} is {null}";
                    var response = ApiResponseUtils.CustomResponse(false, message, model);
                    return StatusCode(StatusCodes.Status404NotFound, response);
                }

                var existingCode = await _context.Items.AnyAsync(i => i.ItemCode == model.ItemCode);

                if (existingCode)
                {
                    var message = $"{model} is {null}";
                    var response = ApiResponseUtils.CustomResponse(false, message, model);
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

                    var message = "Item added successfully";
                    var response = ApiResponseUtils.CustomResponse(false, message, model);
                    return StatusCode(StatusCodes.Status200OK, response);
                }
                else
                {
                    var message = "User ID not doesn't exist";
                    var response = ApiResponseUtils.CustomResponse(false, message, model);
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

    }


}

