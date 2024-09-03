using InventorySystem.Data;
using InventorySystem.Models;
using InventorySystem.Utilities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace InventorySystem.Controllers.main
{
    [Authorize]
    [Route("inventory/{username}")]
    public class UsersController(ApplicationDbContext context/*, UserManager<User>? userManager*/) : Controller
    {

        private readonly ApplicationDbContext _context = context;

        [Route("dashboard")]
        [HttpGet]
        public async Task<IActionResult> UserDashboard(string username, int page = 1)
        {


            // Retrieve UserID of logged in user from session
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (userIdClaim == null || !int.TryParse(userIdClaim, out var userId))
            {
                // Handle the case where the UserID is not available or invalid
                return RedirectToAction("AccessDenied");
            }

            const int pageSize = 24; // Number of items per page
            // Retrieve items associated with the logged-in user
            var itemsQuery = _context.Items.Where(i => i.UserId == userId);


            // Calculate total items and total pages
            var totalItems = await itemsQuery.CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            // Get items for the current page
            var items = await itemsQuery
                .OrderBy(i => i.ItemName) // Or any other ordering
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var model = new ItemListViewModel
            {
                Items = items,
                CurrentPage = page,
                TotalPages = totalPages
            };

            ViewBag.SuccessMessage = $"Welcome, {username}!";
            ViewBag.Username = username;
            ViewBag.UserId = userId;

            ViewData["Layout"] = "~/Views/Shared/_DashboardLayout.cshtml";
            ViewData["title"] = "User Dashboard";

            return View(model);
        }

        [Route("dashboard/summary")]
        [HttpGet]
        public IActionResult Summary(string username)
        {
            ViewData["Layout"] = "~/Views/Shared/_DashboardLayout.cshtml";
            ViewData["title"] = "Summary";
            ViewBag.Username = username;
            return PartialView();
        }


        [Route("dashboard/item-view")]
        [HttpGet]
        public async Task<IActionResult> ItemView(string username, int page = 1)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (userIdClaim == null || !int.TryParse(userIdClaim, out var userId))
            {
                return RedirectToAction("AccessDenied");
            }
            const int pageSize = 24;
            var itemsQuery = _context.Items.Where(i => i.UserId == userId);

            var totalItems = await itemsQuery.CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var items = await itemsQuery
                .OrderBy(i => i.ItemName)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var model = new ItemListViewModel
            {
                Items = items,
                CurrentPage = page,
                TotalPages = totalPages
            };
            ViewBag.Username = username;
            ViewData["Layout"] = "~/Views/Shared/_DashboardLayout.cshtml";
            ViewData["title"] = "Item View";
            return PartialView(model);
        }





        // GET: Admin/Details/id?
        [Route("dashboard/details")]
        [HttpGet]
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
            return PartialView(item);
        }
        #endregion




        // GET: Admin/Create
        [Route("add-item")]
        [HttpGet]

        public IActionResult AddItem()
        {
            /*addItemModel.Users = await _context.Users.ToListAsync()
            addItemModel.Items_ = await _context.Items.ToListAsync();*/

            var model = new Item();

            return PartialView(model);
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

            if (id != model.ItemId)
            {
                return NotFound();
            }

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
