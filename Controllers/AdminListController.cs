using InventorySystem.Data;
using InventorySystem.Models;
using InventorySystem.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventorySystem.Controllers
{
    public class AdminListController(ApplicationDbContext context) : Controller
    {

        private readonly ApplicationDbContext _context = context;

        public async Task<IActionResult> AdminViewer()
        {
            var admin = await _context.Admins.ToListAsync();
            return View(admin);
        }

        public IActionResult AdminModify()
        {
            return View();
        }

        public IActionResult AdminDelete()
        {
            return View();
        }

        public IActionResult ViewUsers()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AdminCreate()
        {
            var admin = new Admin();
            return PartialView(admin);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdminCreate([Bind("AdminId, Username, Email, Password, DateCreated")] Admin adminModel)
        {


            adminModel.Username = adminModel.Username?.Trim();
            adminModel.Email = adminModel.Email?.Trim();
            adminModel.Password = adminModel.Password?.Trim();

            if (ModelState.IsValid)
            {

                var usernameExist = await _context.Admins.AnyAsync(u => u.Username == adminModel.Username);
                var emailExist = await _context.Admins.AnyAsync(u => u.Email == adminModel.Email);
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
                    _context.Admins.Add(adminModel);
                    await _context.SaveChangesAsync();


                    return Json(new
                    {
                        isValid = true,
                        html = Helper.RenderRazorViewToString(this, "AdminCreate", adminModel),
                        successMessage = "Successfuly created!"
                    });
                    //return RedirectToAction(nameof(Admin));
                    //return Json(new(ModelState.IsValid = true, html = "")); 
                }
                catch (Exception ex)
                {
                    return Json(new
                    {
                        isValid = false,
                        html = Helper.RenderRazorViewToString(this, "AdminCreate", adminModel),
                        failedMessage = $"An error occured while saving: {ex.Message}"
                    });
                }

            }



            var admin = await _context.SaveChangesAsync();

            return Json(new
            {
                isValid = false,
                html = Helper.RenderRazorViewToString(this, "AdminCreate", adminModel),
                failedMessage = "Failed to create user!"
            });
        }
    }
}
