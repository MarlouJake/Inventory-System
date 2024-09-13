using InventorySystem.Data;
using InventorySystem.Models.DataEntities;
using InventorySystem.Utilities;
using InventorySystem.Utilities.Api;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public async Task<IActionResult> AddItem([FromBody][Bind("ItemId,ItemCode,ItemName,ItemDescription,Status,ItemDateAdded,ItemDateUpdated,FirmwareUpdated,UserId")] Item model)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors)
                                              .Select(e => e.ErrorMessage)
                                              .ToList();
            var url = Url.Action("AddItem", "ServicesApi");
            Messages.PrintUrl(url);
            Console.WriteLine($"Received data: {JsonConvert.SerializeObject(model)}");

            if (model == null)
            {
                var message = $"{model} is {null}";
                var response = ApiResponseUtils.CustomResponse(false, message, model);
                return StatusCode(StatusCodes.Status404NotFound, response);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(new { IsValid = false, errors });
            }

            if (ModelState.IsValid)
            {
                var existingCode = await _context.Items.AnyAsync(i => i.ItemCode == model.ItemCode);

                if (existingCode)
                {
                    return Ok(new
                    {
                        IsValid = false,
                        errormessage = Messages.ItemCodeExists
                    });
                }

                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

                Console.WriteLine($"User ID Claim: {userIdClaim}");

                if (userIdClaim != null && int.TryParse(userIdClaim, out var userId))
                {
                    model.UserId = userId;

                    try
                    {
                        _context.Items.Add(model);
                        await _context.SaveChangesAsync();
                        var items = await _context.Items
                            .Where(i => i.UserId == userId)
                            .ToListAsync();

                        return StatusCode(201, new
                        {
                            IsValid = true,
                            message = "Item added successfully",
                        });
                    }
                    catch (Exception ex)
                    {
                        return StatusCode(500, new { IsValid = false, message = ex.Message });
                    }
                }
                else
                {
                    return StatusCode(404, new { IsValid = false, message = "User ID not found" });

                }
            }

            return BadRequest(new { IsValid = false, errors });
        }

    }


}

