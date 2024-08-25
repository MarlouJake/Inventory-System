using InventorySystem.Data;
using InventorySystem.Models;
using InventorySystem.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace InventorySystem.Controllers.main
{
    [Authorize]
    public class AdminListController(ApplicationDbContext context) : Controller
    {
        private const string AdminDashboardRoute = "admin-dashboard/{username}";
        private readonly ApplicationDbContext _context = context;


        [Route(AdminDashboardRoute)]
        public async Task<IActionResult> AdminViewer()
        {
            var adminIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var adminnameClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

            if (adminIdClaim == null || !int.TryParse(adminIdClaim, out var adminId))
            {
                // Handle the case where the UserID is not available or invalid
                return RedirectToAction("AccessDenied");
            }

            // Retrieve items associated with the logged-in user
            var users = await _context.Users
                .Where(u => u.AdminId == adminId)
                .ToListAsync();

            ViewBag.SuccessMessage = $"Welcome, {adminnameClaim}!";
            ViewBag.Username = adminnameClaim;
            ViewBag.AdminID = adminId;


            ViewData["Layout"] = "~/Views/Shared/_DashboardLayout.cshtml";
            ViewData["Title"] = "Dashboard";

            return View(users);
        }


        [HttpGet]
        [Route("admin/modify")]
        public IActionResult AdminModify()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AdminModify(int id, [Bind("UserId, Username, Email, Password, DateCreated")] User adminModel)
        {
            adminModel.Username = adminModel.Username?.Trim();
            adminModel.Email = adminModel.Email?.Trim();
            adminModel.Password = adminModel.Password?.Trim();


            if (id != adminModel.UserId)
            {
                return NotFound();
            }

            var existingUser = await _context.Users.FindAsync(id);
            if (existingUser == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

                var adminIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

                if (adminIdClaim != null && int.TryParse(adminIdClaim, out var adminId))
                {
                    try
                    {
                        existingUser.Username = adminModel.Username;
                        await _context.SaveChangesAsync();
                        TempData["SuccessMessage"] = "Update Successful!";

                        var users = await _context.Users
                            .Where(u => u.AdminId == adminId)
                            .ToListAsync();

                        return Json(new
                        {
                            isValid = true,
                            html = Helper.RenderRazorViewToString(this, "AdminListView", users),
                            successMessage = "Update successful!"

                        });
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
            }

            return Json(new
            {
                isValid = false,
                html = Helper.RenderRazorViewToString(this, "AdminModify", adminModel),
                failedMessage = "Update failed!"
            });
        }


        [Route("admin/user-table")]
        public IActionResult ViewUsers()
        {
            return PartialView();
        }

        [HttpGet]
        [Route("admin/create")]
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

                var adminIDClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

                if (adminIDClaim != null && int.TryParse(adminIDClaim, out var adminId))
                {
                    try
                    {
                        adminModel.AdminId = adminId;
                        adminModel.Password = adminModel.Password != null ? HashHelper.HashPassword(adminModel.Password) : string.Empty;
                        _context.Users.Add(adminModel);
                        await _context.SaveChangesAsync();

                        var users = await _context.Users
                            .Where(u => u.AdminId == adminId)
                            .ToListAsync();
                        return Json(new
                        {
                            isValid = true,
                            html = Helper.RenderRazorViewToString(this, "AdminListView", users),
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
                else
                {
                    ModelState.AddModelError("", "Invalid Admin ID.");
                    return Json(new
                    {
                        isValid = false,
                        html = Helper.RenderRazorViewToString(this, "AdminCreate", adminModel),
                        failedMessage = "Invalid Admin ID."
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
        [Route("admin/delete")]
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdminDelete(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }


            var adminIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (adminIdClaim != null && int.TryParse(adminIdClaim, out var adminId))
            {
                try
                {
                    _context.Users.Remove(user);
                    await _context.SaveChangesAsync();

                    var users = await _context.Users
                        .Where(u => u.AdminId == adminId)
                        .ToListAsync();

                    return Json(new
                    {
                        isValid = true,
                        html = Helper.RenderRazorViewToString(this, "AdminListView", await _context.Users.ToListAsync()),
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
    }
}
