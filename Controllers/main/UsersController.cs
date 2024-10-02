using InventorySystem.Data;
using InventorySystem.Models.DataEntities;
using InventorySystem.Models.Pagination;
using InventorySystem.Models.Responses;
using InventorySystem.Utilities.Api;
using InventorySystem.Utilities.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace InventorySystem.Controllers.main
{
    [Authorize("RequireUserRole")]
    [Route("{roleName}/inventory/{username}/")]
    public class UsersController(ApplicationDbContext context, GetClaims getClaims) : Controller
    {
        private readonly ApplicationDbContext _context = context;
        private readonly GetClaims _getClaims = getClaims;
        private int? userId;


        // This method is called before every action method in the controller
        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            // Set userId from claims
            userId = _getClaims.GetIdClaim(User);

            if (userId == null)
            {
                actionContext.Result = RedirectToAction("AccessDenied", "Home");
            }


            // Call the base method
            base.OnActionExecuting(actionContext);
        }

        [Route("dashboard")]
        [HttpGet]
        public async Task<IActionResult> UserDashboard(string roleName, string username, int page = 1)
        {
            var user = await _context.Users.FindAsync(userId);

            // Optionally check if the roleName matches one of the user's roles
            var userRoles = await _context.UserRoles
                .Where(ur => ur.UserId == userId)
                .Select(ur => ur.Role!.Name)
                .ToListAsync();

            bool userNull = user == null;
            bool hasUserRole = !userRoles.Contains(roleName);

            if (userNull || hasUserRole)
            {
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
                TotalPages = totalPages,
                Category = "All"
            };

            ViewBag.SuccessMessage = $"Welcome, {username}!";
            ViewBag.Username = username;
            ViewBag.UserId = userId;
            ViewBag.PageSize = pageSize;

            ViewData["Layout"] = "~/Views/Shared/_DashboardLayout.cshtml";
            ViewData["title"] = "User Dashboard";

            Console.WriteLine("UserId dashboard: {0}", userId);
            return View(model);
        }


        [Route("summary")]
        [HttpGet]
        public IActionResult Summary(string username)
        {
            ViewData["Layout"] = "~/Views/Shared/_DashboardLayout.cshtml";
            ViewData["title"] = "Summary";
            ViewBag.Username = username;
            return PartialView();
        }


        [Route("requests")]
        [HttpGet]
        public IActionResult Requests(string username)
        {
            ViewData["Layout"] = "~/Views/Shared/_DashboardLayout.cshtml";
            ViewData["title"] = "Requests";
            ViewBag.Username = username;
            return PartialView();
        }

        /*
        [Route("dashboard/item-view")]
        [HttpGet]
        public async Task<IActionResult> ItemView(string username, int page = 1)
        {
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
                TotalPages = totalPages,
                Category = "All"
            };
            ViewBag.Username = username;


            ViewData["title"] = "Item View";
            return PartialView(model);
        }*/

        [Route("dashboard/item-view/all")]
        [HttpGet]
        public async Task<IActionResult> ItemViewAll(string username, int page = 1)
        {
            int? id = userId;
            const int pageSize = 24;
            var itemsQuery = _context.Items.Where(i => i.UserId == id);

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
                TotalPages = totalPages,
                Category = "All"
            };
            ViewBag.Username = username;


            ViewData["title"] = "Item View";
            Console.WriteLine("UserId view all: {0}", userId);
            Console.WriteLine("Total Items found in view all: {0}", totalItems);
            return PartialView(model);
        }


        [Route("dashboard/item-view/category")]
        [HttpGet]
        public async Task<IActionResult> CategoryView(string username, string category, int page = 1)
        {

            const int pageSize = 24;

            var itemsQuery = _context.Items.Where(i => i.UserId == userId);

            if (category != "All")
            {
                itemsQuery = _context.Items.Where(i => i.Category == category && i.UserId == userId);
            }

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
                TotalPages = totalPages,
                Category = "All"
            };
            ViewBag.Username = username;

            Console.WriteLine("UserId category view: {0}", userId);
            Console.WriteLine("Total Items found in category view: {0}", totalItems);
            ViewData["title"] = "Item View";
            return PartialView("ItemViewAll", model);
        }


        [Route("dashboard/search/")]
        [HttpGet]
        public async Task<IActionResult> Search(string itemcode, string category, int page = 1)
        {
            int pageSize = 24;
            ApiResponse response = null!;
            string message = "";
            var redirectUrl = "";

            var items = _context.Items
                               .Where(i => i.UserId == userId); // Filter by userId


            if (!string.IsNullOrEmpty(itemcode))
            {
                items = items.Where(i => i.ItemCode!.StartsWith(itemcode) && i.UserId == userId); // Filters items by code             
            }

            if (category != "All")
            {
                items = items.Where(i => i.Category == category && i.UserId == userId);
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
                TotalPages = totalPages,
                Category = category
            };

            if (!model.Items.Any())
            {
                redirectUrl = Url.Action("Search", "Users");
                message = "No item(s) found";
                response = ApiResponseUtils.SuccessResponse(model.Items!, message, redirectUrl!);
                return StatusCode(StatusCodes.Status404NotFound, response);
            }
            ViewData["Category"] = category;
            Console.WriteLine("UserId search: {0}", userId);
            Console.WriteLine("Total Items found in search: {0}", totalItems);
            return PartialView("ItemViewAll", model);
        }


        // GET: Admin/Details/id?
        [Route("dashboard/details/{id?}")]
        [HttpGet]
        public async Task<IActionResult> ViewDetails(int? id, string username, string roleName)
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
            ViewBag.Username = username;
            ViewBag.RoleName = roleName;
            ViewBag.Category = item.Category;
            return PartialView(item);
        }



        // GET: Admin/Create
        [Route("dashboard/add-item/")]
        [HttpGet]
        public IActionResult AddItem()
        {
            var item = new Item();
            return PartialView(item);
        }


        [Route("dashboard/modify/{id?}")]
        [HttpGet]
        public async Task<IActionResult> Update(int? id)
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

        [HttpGet]
        [Route("dashboard/remove/{id?}")]
        public async Task<IActionResult> Delete(int? id)
        {

            Console.WriteLine("User ID: {0}", userId);
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


            ViewBag.Category = item.Category;
            ViewBag.Claim = userId;

            return PartialView(item);
        }

        [HttpGet]
        [Route("dashboard/import-file")]
        public IActionResult ImportFile()
        {
            return PartialView();
        }

        //Partials Views for Modal Update, ViewDetails, Create
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

        [Route("dashboard/update/partial_view_has_firmwareview")]
        [HttpGet]
        public IActionResult HasFirmwareView()
        {
            return PartialView("~/Views/Users/components/HasFirmwareView.cshtml");
        }

        [Route("dashboard/update/partial_view_no_firmwareview")]
        [HttpGet]
        public IActionResult NoFirmwareView()
        {
            return PartialView("~/Views/Users/components/NoFirmwareView.cshtml");
        }

        [Route("dashboard/update/partial_view_has_firmwaredelete")]
        [HttpGet]
        public IActionResult HasFirmwareDelete()
        {
            return PartialView("~/Views/Users/components/HasFirmwareDelete.cshtml");
        }

        [Route("dashboard/update/partial_view_no_firmwaredelete")]
        [HttpGet]
        public IActionResult NoFirmwareDelete()
        {
            return PartialView("~/Views/Users/components/NoFirmwareDelete.cshtml");
        }
    }
}
