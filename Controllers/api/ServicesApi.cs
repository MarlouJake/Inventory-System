using InventorySystem.Data;
using InventorySystem.Models.DataEntities;
using InventorySystem.Models.Responses;
using InventorySystem.Utilities;
using InventorySystem.Utilities.Api;
using InventorySystem.Utilities.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;

namespace InventorySystem.Controllers.api
{
    [Route("api/u/services/")]
    [ApiController]
    public class ServicesApi(ApplicationDbContext context, GetClaims getClaims) : ControllerBase
    {
        private readonly ApplicationDbContext _context = context;
        private readonly GetClaims _getClaims = getClaims;
        private int? userId;
        private string? message;
        private ApiResponse? response;
        private string? redirectTo;
        public async Task OnActionExecuting(ActionExecutingContext context)
        {
            // Set userId from claims
            userId = _getClaims.GetIdClaim(User);

            // If userId is null, return a 403 Forbidden response
            if (userId == null)
            {
                message = "User ID does not exist";
                response = ApiResponseUtils.CustomResponse(false, message, userId);

                context.Result = new JsonResult(response) // Respond with a JSON result
                {
                    StatusCode = StatusCodes.Status403Forbidden // Set the status code to 403
                };

                Console.Error.WriteLine(message);
                Console.WriteLine("User ID Claim: {0}", userId);
                return; // Stop further action execution
            }

            context.HttpContext.Items["UserId"] = userId;

            await Task.CompletedTask;
        }



        [HttpPost("add")]
        public async Task<IActionResult> AppendItem([FromBody][Bind("ItemId,ItemCode,ItemName,ItemDescription,Status,FirmwareUpdated,Category,ItemDateAdded,ItemDateUpdated,UserId")] Item model)
        {

            try
            {
                var url = Url.Action("AppendItem", "ServicesApi");
                Messages.PrintUrl(url);
                Console.WriteLine($"Received data: {JsonConvert.SerializeObject(model)}");

                if (model == null)
                {
                    message = $"Data is {null}";
                    response = ApiResponseUtils.CustomResponse(false, message, model);
                    Console.Error.WriteLine(message);
                    return StatusCode(StatusCodes.Status404NotFound, response);
                }

                userId = _getClaims.GetIdClaim(User);

                if (userId == null)
                {
                    var message = "User ID is not available.";
                    var response = ApiResponseUtils.CustomResponse(false, message, null);
                    return StatusCode(StatusCodes.Status403Forbidden, response);
                }

                var existingCode = await _context.Items.AnyAsync(i => i.ItemCode == model.ItemCode && i.UserId == userId);

                if (existingCode)
                {
                    message = "Item code already in use";
                    response = ApiResponseUtils.CustomResponse(false, message, model);
                    Console.Error.WriteLine(message);
                    return StatusCode(StatusCodes.Status409Conflict, response);
                }



                model.UserId = userId;

                _context.Items.Add(model);
                await _context.SaveChangesAsync();


                redirectTo = "dashboard/item-view/all";
                message = "Add successful";

                Console.WriteLine($"Inserted data: {JsonConvert.SerializeObject(model)}");
                Console.WriteLine(message);
                Console.WriteLine("URL: {0}", redirectTo);
                Console.WriteLine("User ID Claim in create: {0}", userId);
                response = ApiResponseUtils.SuccessResponse(null!, message, redirectTo);
                return StatusCode(StatusCodes.Status201Created, response);


            }
            catch (MySqlException sqlEx)
            {
                message = "An error occurred while connecting to the database.";

                #region --Console Logger--
                Console.Error.WriteLine($"\nMySQL Exception Caught: {sqlEx}\n");
                #endregion

                response = ApiResponseUtils.CustomResponse(false, message, null);
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
            catch (Exception ex)
            {
                message = "An unknown error occurred.";

                #region --Console Logger--
                Console.Error.WriteLine($"\nException Caught: {ex}\n");
                #endregion

                response = ApiResponseUtils.CustomResponse(false, message, null);
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        [HttpPut("modify/{id?}")]
        public async Task<IActionResult> ModifyItem([FromBody][Bind("ItemId,ItemCode,ItemName,ItemDescription,Status,FirmwareUpdated,Category,ItemDateUpdated,UserId")] Item model, int id)
        {
            int Status404 = StatusCodes.Status404NotFound;
            int Status500 = StatusCodes.Status500InternalServerError;
            id = model.ItemId;
            var url = Url.Action("ModifyItem", "ServicesApi");
            Messages.PrintUrl(url);
            Console.WriteLine($"Received data: {JsonConvert.SerializeObject(model)}");
            Console.WriteLine("input id: {0}", model.ItemId);
            try
            {
                userId = _getClaims.GetIdClaim(User);

                if (userId == null)
                {
                    var message = "User ID is not available.";
                    var response = ApiResponseUtils.CustomResponse(false, message, null);
                    return StatusCode(StatusCodes.Status403Forbidden, response);
                }

                if (model == null)
                {
                    message = $"Data is {null}";
                    response = ApiResponseUtils.CustomResponse(false, message, model);
                    Console.Error.WriteLine(message);
                    return StatusCode(Status404, response);
                }

                var existingItem = await _context.Items.FindAsync(id);
                if (existingItem == null)
                {
                    message = $"Item not found: {existingItem}, {id}";
                    response = ApiResponseUtils.CustomResponse(false, message, model);
                    Console.Error.WriteLine(message);
                    return StatusCode(Status404, response);
                }

                // Detach the existing entity to avoid tracking conflicts
                _context.Entry(existingItem).State = EntityState.Detached;

                // Check if the new ItemCode is different from the existing ItemCode
                if (existingItem.ItemCode != model.ItemCode)
                {
                    // Check if the new ItemCode already exists in the database
                    var existingCode = await _context.Items.AnyAsync(i => i.ItemCode == model.ItemCode && i.UserId == userId);

                    if (existingCode)
                    {
                        message = "Item code already in use";
                        response = ApiResponseUtils.CustomResponse(false, message, model);
                        Console.Error.WriteLine(message);
                        return StatusCode(StatusCodes.Status409Conflict, response);
                    }
                }

                model.UserId = userId;
                model.ItemDateAdded = existingItem.ItemDateAdded;

                _context.Items.Update(model);
                await _context.SaveChangesAsync();

                var items = await _context.Items
                    .Where(i => i.UserId == userId)
                    .ToListAsync();

                redirectTo = "dashboard/item-view/all";
                message = "Update successful";
                Console.WriteLine(message);
                Console.WriteLine("URL: {0}", redirectTo);
                Console.WriteLine("User ID Claim: {0}", userId);
                response = ApiResponseUtils.SuccessResponse(null!, message, redirectTo);
                return StatusCode(StatusCodes.Status200OK, response);

            }
            catch (MySqlException sqlEx)
            {
                message = "An error occurred while connecting to the database.";

                #region --Console Logger--
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Error.WriteLine($"\nMySQL Exception Caught: {sqlEx}\n");
                Console.ResetColor();
                #endregion

                response = ApiResponseUtils.CustomResponse(false, message, null);
                return StatusCode(Status500, response);
            }
            catch (Exception ex)
            {
                message = "An unknown error occurred.";

                #region --Console Logger--
                Console.Error.WriteLine($"\nException Caught: {ex}\n");
                #endregion

                response = ApiResponseUtils.CustomResponse(false, message, null);
                return StatusCode(Status500, response);
            }
        }


#pragma warning disable ASP0018 // Unused route parameter
        [HttpDelete("remove-confirm-multiple/{id?}")]
#pragma warning restore ASP0018 // Unused route parameter
        public async Task<IActionResult> MultipleRemoveConfirm([FromBody] int[]? ids)
        {

            int Status404 = StatusCodes.Status404NotFound;
            int Status500 = StatusCodes.Status500InternalServerError;

            try
            {
                // Check if any IDs were provided
                if (ids == null || ids.Length == 0)
                {
                    message = "No IDs provided for deletion.";
                    response = ApiResponseUtils.CustomResponse(false, message, null);
                    return await Task.FromResult(StatusCode(Status404, response));
                }


                foreach (var id in ids)
                {
                    var item = await _context.Items.FindAsync(id);
                    if (item == null)
                    {
                        message = $"Item with ID {id} doesn't exist";
                        Console.WriteLine(message);
                        continue;
                    }
                    Console.WriteLine("Selected id: {0}", id);
                    _context.Items.Remove(item);
                }

                await _context.SaveChangesAsync();

                var items = await _context.Items
                        .Where(i => i.UserId == userId)
                        .ToListAsync();

                string redirectTo = "dashboard/item-view/all";
                message = "Delete successful";

                Console.WriteLine(message);
                Console.WriteLine("URL: {0}", redirectTo);
                Console.WriteLine("User ID received: {0}", userId);

                response = ApiResponseUtils.SuccessResponse(null!, message, redirectTo);
                return StatusCode(StatusCodes.Status200OK, response);
            }
            catch (MySqlException sqlEx)
            {
                message = "An error occurred while connecting to the database.";
                Console.Error.WriteLine($"\nMySQL Exception Caught: {sqlEx}\n");

                response = ApiResponseUtils.CustomResponse(false, message, null);
                return StatusCode(Status500, response);
            }
            catch (Exception ex)
            {
                message = "An unknown error occurred.";
                Console.Error.WriteLine($"\nException Caught: {ex}\n");

                response = ApiResponseUtils.CustomResponse(false, message, null);
                return StatusCode(Status500, response);
            }
        }

    }


}

