using InventorySystem.Data;
using InventorySystem.Models.DataEntities;
using InventorySystem.Models.Pagination;
using InventorySystem.Models.Responses;
using InventorySystem.Utilities.Api;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace InventorySystem.Controllers.main
{
    [Authorize("RequireUserRole")]
    [Route("{roleName}/inventory/{username}/")]
    public class UsersController(ApplicationDbContext context) : Controller
    {
        private readonly ApplicationDbContext _context = context;

        [Route("dashboard")]
        [HttpGet]
        public async Task<IActionResult> UserDashboard(string roleName, string username, int page = 1)
        {
            // Retrieve UserId of logged-in user from claims
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (userIdClaim == null || !int.TryParse(userIdClaim, out var userId))
            {
                // Handle the case where the UserID is not available or invalid
                return RedirectToAction("AccessDenied", "Home");
            }

            // Ensure the logged-in user matches the username provided in the URL
            var user = await _context.Users.FindAsync(userId);
            if (user == null || user.Username != username)
            {
                // Handle the case where the username doesn't match the logged-in user
                return RedirectToAction("AccessDenied", "Home");
            }

            // Optionally check if the roleName matches one of the user's roles
            var userRoles = await _context.UserRoles
                .Where(ur => ur.UserId == userId)
                .Select(ur => ur.Role!.Name)
                .ToListAsync();

            if (!userRoles.Contains(roleName))
            {
                // Handle the case where the roleName is not associated with the user
                return RedirectToAction("AccessDenied", "Home");
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
            ViewBag.PageSize = pageSize;

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


        [Route("dashboard/requests")]
        [HttpGet]
        public IActionResult Requests(string username)
        {
            ViewData["Layout"] = "~/Views/Shared/_DashboardLayout.cshtml";
            ViewData["title"] = "Requests";
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


            ViewData["title"] = "Item View";
            return PartialView(model);
        }


        // GET: Admin/Details/id?
        [Route("dashboard/details/{id?}")]
        [HttpGet]
        #region --Previous ViewDetails Method--
        public async Task<IActionResult> ViewDetails(int? id, string username, string roleName)
        {

            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (userIdClaim == null || !int.TryParse(userIdClaim, out var userId))
            {
                return RedirectToAction("AccessDenied");
            }

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
            ViewBag.Username = username;
            ViewBag.RoleName = roleName;
            ViewBag.Category = item.Category;
            return PartialView(item);
        }
        #endregion


        // GET: Admin/Create
        [Route("dashboard/add-item/")]
        [HttpGet]
        public IActionResult AddItem(string username)
        {
            var item = new Item();
            return PartialView(item);
        }



        #region --Previous Update Method--
        [Route("dashboard/modify/{id?}")]
        [HttpGet]
        public async Task<IActionResult> Update(int? id, string username)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Items
                .FindAsync(id);

            if (item == null)

            {
                return NotFound();
            }


            return PartialView(item);
        }

        #endregion


        #region --Previous Delete Method--
        [HttpGet]
        [Route("dashboard/remove/{id}")]
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


            return PartialView(item);
        }

        #endregion

        [Route("dashboard/search/")]
        [HttpGet]
        public async Task<IActionResult> Search(string itemcode, int page = 1)
        {
            int pageSize = 24;
            ApiResponse response = null!;
            string message = "";
            var redirectUrl = "";


            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (userIdClaim == null || !int.TryParse(userIdClaim, out var userId))
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            var items = _context.Items
                               .Where(i => i.UserId == userId); // Filter by userId


            if (!string.IsNullOrEmpty(itemcode))
            {
                items = items.Where(i => i.ItemCode!.StartsWith(itemcode)); // Filters items by code
            }

            // Get the total number of filtered items
            int totalItems = await items.CountAsync();

            // Calculate total pages
            int totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            // Skips items to implement pagination, and take the number of items per page
            var paginatedItems = await items
                .OrderBy(i => i.ItemName)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var model = new ItemListViewModel
            {
                Items = paginatedItems,
                CurrentPage = page,
                TotalPages = totalPages
            };

            if (!model.Items.Any())
            {
                redirectUrl = Url.Action("Search", "Users");
                message = "No item(s) found";
                response = ApiResponseUtils.SuccessResponse(model.Items!, message, redirectUrl!);
                return StatusCode(StatusCodes.Status404NotFound, response);
            }

            redirectUrl = Url.Action("Search", "Users");
            return PartialView("ItemView", model);
        }


        [Route("dashboard/update/partial_view_with_firmwareUpdate")]
        [HttpGet]
        public IActionResult HasFirmwareUpdate()
        {
            return PartialView("~/Views/Users/components/HasFirmwareUpdate.cshtml");
        }

        [Route("dashboard/update/partial_view_no_firmwareUpdate")]
        [HttpGet]
        public IActionResult NoFirmwareUpdate()
        {
            return PartialView("~/Views/Users/components/NoFirmwareUpdate.cshtml");
        }

    }
}
