using InventorySystem.Data;
using InventorySystem.Models;
using InventorySystem.Utilities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace InventorySystem.Controllers
{
    [Authorize]
    public class UsersController(ApplicationDbContext context/*, UserManager<User>? userManager*/) : Controller
    {
        private readonly ApplicationDbContext _context = context;

        [Route("dashboard/{username}")]
        public async Task<IActionResult> UserDashboard(string username)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (userIdClaim == null || !int.TryParse(userIdClaim, out var userId))
            {
                // Handle the case where the UserID is not available or invalid
                return RedirectToAction("AccessDenied");
            }

            // Retrieve items associated with the logged-in user
            var items = await _context.Items
                .Where(i => i.UserId == userId)
                .ToListAsync();

            ViewBag.WelcomeMessage = TempData["WelcomeMessage"] as string;
            ViewData["Layout"] = "~/Views/Shared/_DashboardLayout.cshtml";
            ViewData["Title"] = "Dashboard";
            return View(items);

        }

        [Route("dashboard/user-table")]
        public async Task<IActionResult> ItemTable()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            Console.WriteLine($"User ID Claim: {userIdClaim}");
            if (userIdClaim == null || !int.TryParse(userIdClaim, out var userId))
            {
                // Handle the case where the UserID is not available or invalid
                return RedirectToAction("AccessDenied");
            }

            // Retrieve items associated with the logged-in user
            var items = await _context.Items
                .Where(i => i.UserId == userId)
                .ToListAsync();
            return PartialView(items);
        }

        // GET: Admin/Details/id?
        [Route("dashboard/details")]
        #region --Previous ViewDetails Method--
        public async Task<IActionResult> ViewDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Items
                .FirstOrDefaultAsync(m => m.ItemId == id);
            if (item == null)
            {
                return NotFound();
            }
            //user.Password = user.Password != null ? HashHelper.HashPassword(user.Password) : string.Empty;
            return View(item);
        }

        #endregion




        // GET: Admin/Create
        [HttpGet]
        [Route("dashboard/add-item")]
        public IActionResult AddItem()
        {
            /*addItemModel.Users = await _context.Users.ToListAsync()
            addItemModel.Items_ = await _context.Items.ToListAsync();*/

            var model = new Item();

            return PartialView(model);


        }

        [HttpGet]
        public JsonResult GetStatuses()
        {
            var statuses = new List<SelectListItem>
            {
                //new() {Value = "--Select Status--", Text = "--Select Status--"},
                new () { Value = "Complete", Text = "Complete" },
                new() { Value = "Incomplete(Usable)", Text = "Incomplete(Usable)" },
                new () { Value = "Incomplete(Unusable)", Text = "Incomplete(Unusable)" }
            };

            return Json(statuses);
        }



        [HttpGet]
        public JsonResult GetOptions()
        {
            var options = new List<SelectListItem>
            {
                //new() {Value = "--Select Status--", Text = "--Select Status--"},
                 new () { Value = "N/A", Text = "N/A" },
                new () { Value = "YES", Text = "YES" },
                new() { Value = "NO", Text = "NO" }

            };

            return Json(options);
        }

        /*private static string[] ConsoleOutputs(Item model)
        {
            // Define a string array with each element being a formatted output string
            string[] arr =
            [
                $"Item code: {model.ItemCode}",
                $"Item name: {model.ItemName}",
                $"Item description: {model.ItemDescription}",
                $"Item status: {model.Status}",
                $"Item additional info: {model.AdditionalInfo}",
                $"Item firmware updated: {model.FirmwareUpdated}",
                $"Item date added: {model.ItemDateAdded}",
                $"Item date updated: {model.ItemDateUpdated}",
                $"Item user id: {model.UserId}"
            ];

            // Print each element of the string array
            foreach (var output in arr)
            {
                Console.WriteLine(output);
            }

            // Return the string array
            return arr;
        }*/

        // POST: Users/Create/id
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddItem([Bind("ItemId,ItemCode,ItemName,ItemDescription,Status,AdditionalInfo,ItemDateAdded,ItemDateUpdated,FirmwareUpdated,UserId")] Item model)
        {
            if (model == null)
            {
                ModelState.AddModelError("", "Item data is null");
                return Json(new
                {
                    isValid = false,
                    html = Helper.RenderRazorViewToString(this, "AddItem", model),
                    failedMessage = "Item data is null"
                });
            }

            //ConsoleOutputs(model);

            if (string.IsNullOrWhiteSpace(model.ItemCode) || model.ItemCode.Contains(' ')
                || string.IsNullOrWhiteSpace(model.FirmwareUpdated) || model.FirmwareUpdated.Contains(' ')
                || string.IsNullOrWhiteSpace(model.Status) || model.Status.Contains("--Select Status--"))
            {
                ModelState.AddModelError("", "Required fields should not be empty");
                return Json(new
                {
                    isValid = false,
                    html = Helper.RenderRazorViewToString(this, "AddItem", model),
                    failedMessage = "Required fields should not be empty"
                });
            }


            if (ModelState.IsValid)
            {
                var existingCode = await _context.Items.AnyAsync(i => i.ItemCode == model.ItemCode);

                if (existingCode)
                {
                    TempData["ErrorMessage"] = "Code already exists.";
                    return Json(new
                    {
                        isValid = false,
                        html = Helper.RenderRazorViewToString(this, "AddItem", model),
                        failedMessage = "Code already exists!"
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
                        TempData["SuccessMessage"] = "Item added successfully.";
                        return Json(new
                        {
                            isValid = true,
                            html = Helper.RenderRazorViewToString(this, "ItemTable", items),
                            successMessage = "Successfully added!",
                        });
                    }
                    catch (Exception ex)
                    {
                        return StatusCode(500, new { success = false, message = ex.Message });
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Invalid User ID.");
                    return Json(new
                    {
                        isValid = false,
                        html = Helper.RenderRazorViewToString(this, "AddItem", model),
                        failedMessage = "Invalid User ID."
                    });
                }
            }

            TempData["ErrorMessage"] = "Failed to add item!";

            return Json(new
            {
                isValid = false,
                html = Helper.RenderRazorViewToString(this, "AddItem", model),
                failedMessage = "Failed to add item!",

            });
        }







        // GET: Admin/Update/id?
        #region --Previous Update Method--
        [HttpGet]
        [Route("dashboard/update-item")]
        public async Task<IActionResult> Update(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Items.FindAsync(id); // logic to get the user data

            if (item == null)

            {
                return NotFound();
            }


            return PartialView(item);
        }

        #endregion





        // POST: Admin/Update/id?
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, [Bind("ItemId,ItemCode,ItemName,ItemDescription,Status,AdditionalInfo,ItemDateUpdated,FirmwareUpdated")] Item model)
        {
            //ViewData["IsCreationContext"] = false;
            // Retrieve the success message from TempData if it exists


            if (id != model.ItemId)
            {
                return NotFound();
            }

            //var existingUser = await _context.Users.FindAsync(id);
            var existingItem = await _context.Items.FindAsync(id);
            if (existingItem == null)
            {
                return NotFound();
            }



            if (ModelState.IsValid)
            {
                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                Console.WriteLine($"User ID Claim: {userIdClaim}");

                if (userIdClaim != null && int.TryParse(userIdClaim, out var userId))
                {

                    try
                    {
                        //_context.Entry(existingUser).CurrentValues.SetValues(user);
                        existingItem.ItemCode = model.ItemCode;
                        existingItem.ItemDateUpdated = DateTime.Now;

                        //_context.Update(user);

                        await _context.SaveChangesAsync();
                        //return RedirectToAction(nameof("UserTable"));
                        TempData["SuccessMessage"] = "Update Successful!";

                        var items = await _context.Items
                            .Where(i => i.UserId == userId)
                            .ToListAsync();

                        return Json(new
                        {
                            isValid = true,
                            html = Helper.RenderRazorViewToString(this, "ItemTable", items),
                            successMessage = "Update successful!"

                        });
                    }
                    catch (Exception ex)
                    {
                        return StatusCode(500, new { success = false, message = ex.Message });
                    }
                }

            }

            //return PartialView(user);     
            return Json(new
            {
                isValid = false,
                html = Helper.RenderRazorViewToString(this, "Update", model),
                failedMessage = "Update failed!"
            });
        }






        // GET: Admin/Delete/id?
        #region --Previous Delete Method--
        [HttpGet]
        [Route("dashboard/delete-item")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Items
                .FirstOrDefaultAsync(m => m.ItemId == id);
            if (item == null)
            {
                return NotFound();
            }

            /*
            return Json(new
            {
                isValid = true,
                html = Helper.RenderRazorViewToString(this, "Delete", await _context.Users.ToListAsync())
            }); */

            return PartialView(item);
        }

        #endregion


        #region --Previous DeleteConfirmed Method--

        // POST: Admin/Delete/id?
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.Items.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            Console.WriteLine($"User ID Claim: {userIdClaim}");

            if (userIdClaim != null && int.TryParse(userIdClaim, out var userId))
            {
                try
                {
                    _context.Items.Remove(item);
                    await _context.SaveChangesAsync();
                    var items = await _context.Items
                            .Where(i => i.UserId == userId)
                            .ToListAsync();
                    return Json(new
                    {
                        isValid = true,
                        html = Helper.RenderRazorViewToString(this, "ItemTable", items),
                        successMessage = "Deletion successful!"
                    });
                }
                catch (Exception ex)
                {
                    return StatusCode(500, new { success = false, message = ex.Message });
                }
            }
            else
            {
                return StatusCode(500, new { success = false, message = "Deletion Failed" });
            }

        }

        #endregion

        [Route("dashboard/logout")]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home"); // Redirect to a safe page or login page
        }

        /*
        #pragma warning disable IDE0051 // Remove unused private members
                private bool UserExists(int id)
        #pragma warning restore IDE0051 // Remove unused private members

                {
                    return _context.Users.Any(e => e.UserId == id);
                }


            }
        */
    }
}


#region --Failure ViewDetails--
/*
public async Task<IActionResult> ViewDetails(string encryptedId, byte[] key, byte[] iv)
{

    if (key == null || iv == null)
    {
        return StatusCode(StatusCodes.Status500InternalServerError, "Encryption key or IV not provided.");
    }

    int id = IdEncryptor.DecryptId(encryptedId, key, iv);

    var user = await _context.Users.FirstOrDefaultAsync(m => m.UserId == id);
    if (user == null)
    {
        return NotFound();
    }

    return View(user);

}
*/
#endregion


#region --Failure Update--
/*
public async Task<IActionResult> Update(string id)
{
    int userId;

    try
    {
        userId = IdHasher.UnhashId(id);
    }

    catch
    {
        return NotFound();
    }

    var user = await _context.Users.FindAsync(userId);
    if (user == null)
    {
        return NotFound();
    }
    return View(user);

}
*/
#endregion


#region --Failure DeleteConfirmed--
/*
public async Task<IActionResult> DeleteConfirmed(string id)
{
    int userId;

    try
    {
        userId = IdHasher.UnhashId(id);
    }

    catch
    {
        return NotFound();
    }

    var user = await _context.Users.FindAsync(userId);
    if (user == null)
    {
        return NotFound();
    }

    await _context.SaveChangesAsync();
    return RedirectToAction(nameof(Admin));
}
*/
#endregion