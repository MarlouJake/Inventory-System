using InventorySystem.Data;
using InventorySystem.Models;
using InventorySystem.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace InventorySystem.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class UsersController(ApplicationDbContext context/*, UserManager<User>? userManager*/) : Controller
    {
        private readonly ApplicationDbContext _context = context;

        // GET: Admin     
        public async Task<IActionResult> UserDashboard()
        {

            //var user = await _context.Users.ToListAsync();
            var items = await _context.Items.ToListAsync();

            // Retrieve the success message from TempData if it exists
            ViewBag.SuccessMessage = TempData["SuccessMessage"] as string;

            return View(items);
        }


        // GET: Admin/Details/id?
        #region --Previous ViewDetails Method--
        public async Task<IActionResult> ViewDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (user == null)
            {
                return NotFound();
            }
            //user.Password = user.Password != null ? HashHelper.HashPassword(user.Password) : string.Empty;
            return View(user);
        }

        #endregion




        // GET: Admin/Create
        [HttpGet]
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

        private static string[] ConsoleOutputs(Item model)
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
        }

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

            ConsoleOutputs(model);

            if (string.IsNullOrWhiteSpace(model.ItemCode) || model.ItemCode.Contains(' '))
            {
                ModelState.AddModelError("", "Item code cannot contain spaces");
                return Json(new
                {
                    isValid = false,
                    html = Helper.RenderRazorViewToString(this, "AddItem", model),
                    failedMessage = "Item code cannot contain spaces"
                });
            }
            if (string.IsNullOrWhiteSpace(model.FirmwareUpdated) || model.FirmwareUpdated.Contains(' '))
            {
                ModelState.AddModelError("", "Selecet firmware update option first");
                return Json(new
                {
                    isValid = false,
                    html = Helper.RenderRazorViewToString(this, "AddItem", model),
                    failedMessage = "Selecet firmware update option first"
                });
            }
            if (string.IsNullOrWhiteSpace(model.Status) || model.Status.Contains("--Select Status--"))
            {
                ModelState.AddModelError("", "Select status first");
                return Json(new
                {
                    isValid = false,
                    html = Helper.RenderRazorViewToString(this, "AddItem", model),
                    failedMessage = "Select status first"
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

                try
                {
                    model.UserId = 0;
                    _context.Items.Add(model);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Item added successfully.";
                    return Json(new
                    {
                        isValid = true,
                        html = Helper.RenderRazorViewToString(this, "UserTable", await _context.Items.ToListAsync()),
                        successMessage = "Successfully added!"
                    });
                }
                catch (Exception ex)
                {
                    return StatusCode(500, new { success = false, message = ex.Message });
                }
            }

            TempData["ErrorMessage"] = "Failed to add item!";

            return Json(new
            {
                isValid = false,
                html = Helper.RenderRazorViewToString(this, "AddItem", model),
                failedMessage = "Failed to add item!"
            });
        }






        // GET: Admin/Update/id?
        #region --Previous Update Method--
        [HttpGet]
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

                //_context.Entry(existingUser).CurrentValues.SetValues(user);
                existingItem.ItemCode = model.ItemCode;


                //_context.Update(user);

                await _context.SaveChangesAsync();
                //return RedirectToAction(nameof("UserTable"));
                TempData["SuccessMessage"] = "Update Successful!";
                return Json(new
                {
                    isValid = true,
                    html = Helper.RenderRazorViewToString(this, "UserTable", await _context.Items.ToListAsync()),
                    successMessage = "Update successful!"

                });
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
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.Items.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            _context.Items.Remove(item);
            await _context.SaveChangesAsync();

            return Json(new
            {
                isValid = true,
                html = Helper.RenderRazorViewToString(this, "UserTable", await _context.Items.ToListAsync()),
                successMessage = "Deletion successful!"
            });
        }

        #endregion




        private bool UserExists(int id)

        {
            return _context.Users.Any(e => e.UserId == id);
        }


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