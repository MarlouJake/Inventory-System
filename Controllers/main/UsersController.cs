using InventorySystem.Data;
using InventorySystem.Models.DataEntities;
using InventorySystem.Models.Responses;
using InventorySystem.Utilities.Api;
using InventorySystem.Utilities.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;


namespace InventorySystem.Controllers.main
{
    [Authorize("RequireUserRole")]
    [Route("{roleName}/ims/{username}/")]
    public class UsersController(ApplicationDbContext context, GetClaims getClaims, ItemQuery query, ValidateArrayOfId validateArrayOfId) : Controller
    {
        private readonly ApplicationDbContext _context = context;
        private readonly GetClaims _getClaims = getClaims;
        private readonly ItemQuery _query = query;
        private int? userId;
        private readonly ValidateArrayOfId _validateArrayOfId = validateArrayOfId;
        private ApiResponse response = null!;
        private string message = "";
        private string? redirectUrl = "";


        // This method is called before every action method in the controller
        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            // Set userId from claims
            userId = _getClaims.GetIdClaim(User);

            if (userId == null)
            {
                actionContext.Result = RedirectToAction("AccessDenied", "Home");
            }

            base.OnActionExecuting(actionContext);
        }


        //
        //
        // Main View 
        //
        //

        [Route("content")]
        [HttpGet]
        public IActionResult ContentHandler(string username)
        {
            ViewData["title"] = "IMS";
            ViewBag.Username = username;
            return PartialView();
        }



        [Route("dashboard")]
        [HttpGet]
        public IActionResult Dashboard(string username)
        {
            ViewData["title"] = "Dashboard";
            ViewBag.Username = username;
            return PartialView();
        }


        [Route("inventory")]
        [HttpGet]
        public async Task<IActionResult> Inventory(string username, int page = 1, string category = "All")
        {
            int? id = userId;

            var model = await _query.Pagination(id, page, category, "ItemView");
            var pageSize = _query.GetPageSize();

            ViewData["Layout"] = null;
            ViewData["title"] = "Inventory";

            ViewBag.SuccessMessage = $"Welcome, {username}!";
            ViewBag.Username = username;
            ViewBag.UserId = userId;
            ViewBag.CurrentPage = model.CurrentPage;
            ViewBag.PageSize = pageSize;

            return PartialView(model);
        }


        [Route("requests")]
        [HttpGet]
        public IActionResult Requests(string username)
        {
            ViewData["Layout"] = null;
            ViewData["title"] = "Requests";
            ViewBag.Username = username;
            return PartialView();
        }


        [Route("inventory/items/uncategorized")]
        [HttpGet]
        public async Task<IActionResult> ItemsView(string username, int page = 1, string category = "All")
        {
            int? id = userId;
            var model = await _query.Pagination(id, page, category, "ItemView");

            ViewData["Layout"] = null;
            ViewData["title"] = "Item View";

            ViewBag.Username = username;

            return PartialView(model);
        }


        [Route("inventory/items/categorized")]
        [HttpGet]
        public async Task<IActionResult> CategoryView(string username, string category, int page = 1)
        {
            int? id = userId;
            var model = await _query.Pagination(id, page, category, "ItemView");

            ViewData["Layout"] = null;
            ViewData["title"] = "Item View";

            ViewBag.Username = username;
            ViewBag.Category = category;

            return PartialView("ItemsView", model);
        }


        //
        //
        // Services
        //
        //
        //


        [Route("inventory/search/")]
        [HttpGet]
        public async Task<IActionResult> SearchView(string itemcode, string category, int page = 1)
        {

            /*
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
            };*/
            int? id = userId;
            var model = await _query.Pagination(id, page, category, "ItemView", itemcode);

            if (!model.Items!.Any())
            {
                redirectUrl = Url.Action("Search", "Users");
                message = "No item(s) found";
                response = ApiResponseUtils.SuccessResponse(model.Items!, message, redirectUrl!);
                return StatusCode(StatusCodes.Status404NotFound, response);
            }


            Console.WriteLine("Category: {0}", category);

            ViewData["Category"] = category;


            return PartialView("CategoryView", model);
        }



        [Route("inventory/add-item/")]
        [HttpGet]
        public IActionResult AddItem()
        {
            var item = new Item();
            ViewData["Layout"] = null;
            return PartialView(item);
        }


        [Route("inventory/modify/{id?}")]
        [HttpGet]
        public async Task<IActionResult> Update(int? id)
        {


            if (id == null)
            {
                return NotFound();
            }

            var item = await _query.FindItemAsync(id);

            if (item == null)

            {
                return NotFound();
            }

            ViewData["Layout"] = null;

            return PartialView(item);
        }


        [Route("inventory/details/{id?}")]
        [HttpGet]
        public async Task<IActionResult> ViewDetails(int? id, string username, string roleName)
        {

            if (id == null)
            {
                return NotFound();
            }

            var item = await _query.FindItemAsync(id);

            if (item == null)
            {
                return NotFound();
            }


            ViewData["Layout"] = null;

            ViewBag.Username = username;
            ViewBag.RoleName = roleName;
            ViewBag.Category = item.Category;
            ViewBag.ItemId = item.ItemId;

            return PartialView(item);
        }


        [HttpGet]
        [Route("inventory/remove/{id?}")]
        public async Task<IActionResult> Delete(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var item = await _query.FindItemAsync(id);

            if (item == null)
            {
                return NotFound();
            }


            ViewBag.Category = item.Category;
            ViewBag.Claim = userId;

            return PartialView(item);
        }


        [HttpGet]
        [Route("inventory/remove-multiple/{ids?}")]
        public async Task<IActionResult> DeleteMultiple(int[]? ids)
        {

            var result = await _validateArrayOfId.ValidateAsync(ids, _context);

            if (!result.IsValid)
            {
                NotFound();
            }


            ViewBag.Claim = userId;

            return PartialView();
        }





        [HttpGet]
        [Route("inventory/import-file")]
        public IActionResult ImportFile()
        {
            return PartialView();
        }


        //
        //
        //Partials Views for Dashboard
        //
        //
        [Route("dashboard/partial_views/inventory-summary")]
        [HttpGet]
        public IActionResult InventorySummary(int? id)
        {
            ViewData["Layout"] = null;
            var item = new Item();
            return PartialView("~/Views/Users/PartialViews/Dashboard/InventorySummary.cshtml", item);
        }


        //
        //
        //Partials Views for Update, ViewDetails, Create
        //
        //

        [Route("inventory/update/partial_views/with_firmwareUpdate")]
        [HttpGet]
        public IActionResult HasFirmwareUpdate()
        {
            ViewData["Layout"] = null;
            var item = new Item();
            return PartialView("~/Views/Users/PartialViews/Inventory/HasFirmwareUpdate.cshtml", item);
        }

        [Route("inventory/update/partial_views/no_firmwareUpdate")]
        [HttpGet]
        public IActionResult NoFirmwareUpdate()
        {
            ViewData["Layout"] = null;
            var item = new Item();
            return PartialView("~/Views/Users/PartialViews/Inventory/NoFirmwareUpdate.cshtml", item);
        }

        [Route("inventory/details/partial_views/has_firmwareview")]
        [HttpGet]
        public async Task<IActionResult> HasFirmwareView(int? id)
        {
            ViewData["Layout"] = null;
            var item = await _query.FindItemAsync(id);
            return PartialView("~/Views/Users/PartialViews/Inventory/HasFirmwareView.cshtml", item);
        }

        [Route("inventory/details/partial_views/no_firmwareview")]
        [HttpGet]
        public async Task<IActionResult> NoFirmwareView(int? id)
        {
            ViewData["Layout"] = null;
            var item = await _query.FindItemAsync(id);
            return PartialView("~/Views/Users/PartialViews/Inventory/NoFirmwareView.cshtml", item);
        }

        [Route("inventory/delete/partial_views/has_firmwaredelete")]
        [HttpGet]
        public IActionResult HasFirmwareDelete()
        {
            ViewData["Layout"] = null;
            var item = new Item();
            return PartialView("~/Views/Users/PartialViews/Inventory/HasFirmwareDelete.cshtml", item);
        }

        [Route("inventory/delete/partial_views/no_firmwaredelete")]
        [HttpGet]
        public IActionResult NoFirmwareDelete()
        {
            ViewData["Layout"] = null;
            var item = new Item();
            return PartialView("~/Views/Users/PartialViews/Inventory/NoFirmwareDelete.cshtml", item);
        }
    }
}
