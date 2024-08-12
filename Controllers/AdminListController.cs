using InventorySystem.Data;
using InventorySystem.Models;
using InventorySystem.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventorySystem.Controllers
{
    [Authorize]
    [Route("AdminList/[action]")]
    public class AdminListController(ApplicationDbContext context) : Controller
    {

        private readonly ApplicationDbContext _context = context;


        public async Task<IActionResult> AdminViewer()
        {
            var admin = await _context.Users.ToListAsync();
            return View(admin);
        }


        public IActionResult AdminModify()
        {
            return View();
        }



        public IActionResult ViewUsers()
        {
            return PartialView();
        }

        [HttpGet]
        public IActionResult AdminCreate()
        {
            var admin = new User();
            return PartialView(admin);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdminCreate([Bind("UserId, Username, Email, Password, DateCreated")] User adminModel)
        {


            adminModel.Username = adminModel.Username?.Trim();
            adminModel.Email = adminModel.Email?.Trim();
            adminModel.Password = adminModel.Password?.Trim();

            if (ModelState.IsValid)
            {

                var usernameExist = await _context.Users.AnyAsync(u => u.Username == adminModel.Username);
                var emailExist = await _context.Users.AnyAsync(u => u.Email == adminModel.Email);
                if (usernameExist)
                {
                    return Json(new
                    {
                        isValid = false,
                        html = Helper.RenderRazorViewToString(this, "AdminCreate", adminModel),
                        failedMessage = "Username already exists!"
                    });
                }

                if (emailExist)
                {
                    return Json(new
                    {
                        isValid = false,
                        html = Helper.RenderRazorViewToString(this, "AdminCreate", adminModel),
                        failedMessage = "Email already exists!"
                    });
                }

#pragma warning disable CS8602 // Dereference of a possibly null reference.
                if (adminModel.Username.Contains(' ') || adminModel.Email.Contains(' ') || adminModel.Password.Contains(' '))
                {
                    ModelState.AddModelError("", "Input cannot contain spaces.");
                    return Json(new
                    {
                        isValid = false,
                        html = Helper.RenderRazorViewToString(this, "AdminCreate", adminModel),
                        failedMessage = "Input cannot contain spaces!"
                    });
                }
#pragma warning restore CS8602 // Dereference of a possibly null reference.

                try
                {
                    adminModel.Password = adminModel.Password != null ? HashHelper.HashPassword(adminModel.Password) : string.Empty;
                    _context.Users.Add(adminModel);
                    await _context.SaveChangesAsync();
                    Console.WriteLine("Submitted");
                    return Json(new
                    {
                        isValid = true,
                        html = Helper.RenderRazorViewToString(this, "AdminListView", await _context.Users.ToListAsync()),
                        successMessage = "Successfuly created!"
                    });
                    //return RedirectToAction(nameof(Admin));
                    //return Json(new(ModelState.IsValid = true, html = "")); 
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message.ToString());
                    return Json(new
                    {
                        isValid = false,
                        html = Helper.RenderRazorViewToString(this, "AdminCreate", adminModel),
                        failedMessage = $"An error occured while saving: {ex.Message}"
                    });
                }

            }


            Console.WriteLine("Failed to submit");


            return Json(new
            {
                isValid = false,
                html = Helper.RenderRazorViewToString(this, "AdminCreate", adminModel),
                failedMessage = "Failed to create user!"
            });
        }

        // GET: Admin/Delete/id?
        #region --Previous Delete Method--
        [HttpGet]
        public async Task<IActionResult> AdminDelete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.UserId == id);
            if (user == null)
            {
                return NotFound();
            }


            return PartialView(user);
        }

        #endregion


        #region --Previous DeleteConfirmed Method--

        // POST: Admin/Delete/id?
        [HttpPost, ActionName("AdminDelete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdminDelete(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return Json(new
            {
                isValid = true,
                html = Helper.RenderRazorViewToString(this, "AdminListView", await _context.Users.ToListAsync()),
                successMessage = "Deletion successful!"
            });


        }

        #endregion
    }
}
