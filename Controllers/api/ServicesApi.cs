using InventorySystem.Data;
using InventorySystem.Models;
using InventorySystem.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Security.Claims;

namespace InventorySystem.Controllers.api
{
    [Route("api/services/")]
    [ApiController]
    public class ServicesApi(ApplicationDbContext context) : ControllerBase
    {
        private readonly ApplicationDbContext _context = context;

        [HttpPost("add-item")]
        public async Task<IActionResult> AddItem([FromBody][Bind("ItemId,ItemCode,ItemName,ItemDescription,Status,AdditionalInfo,ItemDateAdded,ItemDateUpdated,FirmwareUpdated,UserId")] Item model)
        {
            var url = Url.Action("AddItem", "ServicesApi");
            Messages.PrintUrl(url);
            Console.WriteLine($"Received data: {JsonConvert.SerializeObject(model)}");

            if (model == null)
            {
                ModelState.AddModelError("", Messages.NotFound);
                /*return Ok(new
                {
                    isValid = false,
                    failedMessage = Messages.NotFound
                });*/
                return BadRequest(Messages.NotFound);
            }

            //Console.WriteLine($"ID:{model.UserId}, Code:{model.ItemCode}, Name: {model.ItemName}, Desc: {model.ItemDescription}, Status: {model.Status}," +
            //      $"Info: {model.AdditionalInfo}, Firmware: {model.FirmwareUpdated}, Date: {model.ItemDateAdded}");

            var itemCode = model.ItemCode ?? string.Empty;
            var firmwareUpdated = model.FirmwareUpdated ?? string.Empty;
            var status = model.Status ?? string.Empty;
            var validatenull = Validation.ValidateNull(itemCode, firmwareUpdated, status);
            var validatespaces = Validation.ValidateSpaces(itemCode, firmwareUpdated, status, " ");
            var selectStatus = status.Contains("--Select Status--");

            switch (true)
            {
                case bool _ when validatenull:
                    return Ok(new { isValid = false, errormessage = Messages.NullOrEmpty });
                case bool _ when validatespaces:
                    return Ok(new { isValid = false, errormessage = Messages.ContainsSpaces });
                case bool _ when selectStatus:
                    return Ok(new { isValid = false, errormessage = Messages.ChooseStatus });
                default:
                    break;
            }

            if (ModelState.IsValid)
            {
                var existingCode = await _context.Items.AnyAsync(i => i.ItemCode == model.ItemCode);

                if (existingCode)
                {
                    return Ok(new
                    {
                        isValid = false,
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
                        //var htmlview = Helper.RenderPartialViewToString("ItemTable", model);
                        return Ok(new
                        {
                            isValid = true,
                            //html = ,
                            successmessage = "Item added successfully",
                        });
                    }
                    catch (Exception ex)
                    {
                        return StatusCode(500, new { success = false, message = ex.Message });
                    }
                }
                else
                {
                    ModelState.AddModelError("", Messages.InvalidUserID);
                    return Ok(new
                    {
                        isValid = false,
                        errormessage = Messages.InvalidUserID
                    });
                }
            }

            return Ok(new
            {
                isValid = false,
                errormessage = Messages.AddFailed
            });
        }

    }


}

