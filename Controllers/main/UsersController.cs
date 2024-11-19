using InventorySystem.Data;
using InventorySystem.Models.DataEntities;
using InventorySystem.Models.Responses;
using InventorySystem.Utilities.Api;
using InventorySystem.Utilities.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ZstdSharp.Unsafe;


namespace InventorySystem.Controllers.main
{
    [Authorize("RequireUserRole")]
    [Route("ims")]
    public class UsersController(ApplicationDbContext context, GetClaims getClaims, ItemQuery query, ValidateArrayOfId validateArrayOfId, HistoryQuery getHistory) : Controller
    {
        private readonly ApplicationDbContext _context = context;
        private readonly GetClaims _getClaims = getClaims;
        private readonly ItemQuery _query = query;
        private readonly HistoryQuery _getHistory = getHistory;
        private int? userId;
        private readonly ValidateArrayOfId _validateArrayOfId = validateArrayOfId;
        private ApiResponse response = null!;
        private string message = "";
        //private string? redirectUrl = "";


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

        
        [HttpGet("content")]
        public IActionResult ContentHandler(string username)
        {
            ViewData["title"] = "IMS";
            ViewBag.Username = username;
            return View();
        }



       
        [HttpGet("dashboard")]
        public async Task<IActionResult> DashboardAsync(string username)
        {
            int? id = userId;
            var model = await _getHistory.Pagination(id, 1);

            ViewData["title"] = "Dashboard";
            ViewBag.Username = username;
            ViewBag.TotalPages = model.TotalPages;
            return PartialView();
        }


        [HttpGet("inventory")]
        public async Task<IActionResult> Inventory(string username, int page = 1, string category = "All")
        {
            int? id = userId;
            _query.SetPageSize(24);
            var model = await _query.Pagination(id, page, category, "");
            var pageSize = _query.GetPageSize();

            ViewData["title"] = "Inventory";
            ViewBag.SuccessMessage = $"Welcome, {username}!";
            ViewBag.Username = username;
            ViewBag.UserId = userId;
            ViewBag.CurrentPage = model.CurrentPage;
            ViewBag.PageSize = pageSize;

            return PartialView(model);
        }

        [HttpGet("requests")]
        public IActionResult Requests(string username)
        {
            ViewData["title"] = "Requests";
            ViewBag.Username = username;
            return PartialView();
        }





        //
        //
        //Partials Views for Inventory
        //
        //

        [Route("inventory/items/uncategorized")]
        [HttpGet]
        public async Task<IActionResult> ItemsView(string username, string itemcode, int page = 1, string category = "All")
        {
            int? id = userId;
            var model = await _query.Pagination(id, page, category, itemcode);

            ViewData["title"] = "Item View";

            ViewBag.Username = username;
            ViewBag.UserId = id;
            return PartialView(model);
        }


        [Route("inventory/item-card")]
        [HttpGet]
        public IActionResult ItemCard(int id)
        {
            return PartialView("~/Views/Users/ItemCard.cshtml");
        }

        [Route("inventory/items/categorized")]
        [HttpGet]
        public async Task<IActionResult> CategoryView(string username, string itemcode , int page = 1, string category = "All")
        {
            int? id = userId;
            var model = await _query.Pagination(id, page, category, itemcode);

            ViewData["title"] = "Item View";

            ViewBag.Username = username;
            ViewBag.Category = category;
                
            if (!model.Items!.Any())
            {
                StatusCode(StatusCodes.Status404NotFound, ApiResponseUtils.CustomResponse(false, "No items found"));
            }

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
            int? id = userId;
            var model = await _query.Pagination(id, page, category, itemcode);

            if (!model.Items!.Any())
            {
                message = "No item(s) found";
                response = ApiResponseUtils.CustomResponse(false, message);
                return StatusCode(StatusCodes.Status404NotFound, response);
            }


            Console.WriteLine("Category: {0}", category);

            ViewData["Category"] = category;


            return PartialView("ItemsView", model);
        }



        [Route("inventory/add-item/")]
        [HttpGet]
        public IActionResult AddItem()
        {
            var item = new Item();
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

            ViewBag.Username = username;
            ViewBag.RoleName = roleName;
            ViewBag.Category = item.Category;
            ViewBag.ItemId = item.ItemId;

            return PartialView(item);
        }


        [HttpGet]
        [Route("inventory/remove/{id?}")]
        public async Task<IActionResult> DeleteMultiple(int[] ids)
        {

            var result = await _validateArrayOfId.ValidateAsync(ids, _context);

            if (!result.IsValid)
            {
                NotFound();
            }

            var items = await _query.FindItemsAsync(ids);

            if (items == null)
            {
                return NotFound();
            }

            ViewBag.Claim = userId;
            
            return PartialView();
        }




        //
        //
        // Partial Views
        //
        //

        [Route("partial_views/nav-dropdown", Name = "NavDropdown")]
        [HttpGet]
        public async Task<IActionResult> NavDropdown()
        {
             return await Task.FromResult(PartialView("~/Views/Users/PartialViews/NavDropdown.cshtml"));
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


        [Route("dashboard/partial_views/inventory-summary", Name = "InventorySummary")]
        [HttpGet]
        public async Task<IActionResult> InventorySummary(string? category = null)
        {
            int? id = userId;
                     
            int robotsCount = await _query.CountTotalItems(id, "Robots");
            int booksCount = await _query.CountTotalItems(id, "Books");
            int materialsCount = await _query.CountTotalItems(id, "Materials");

            var itemsCount = robotsCount + booksCount + materialsCount;

            int[] items = [robotsCount, booksCount, materialsCount];

            ViewBag.ItemsCount = items;
            Console.WriteLine("Total Items: {0}", itemsCount);
            return PartialView("~/Views/Users/PartialViews/Dashboard/InventorySummary.cshtml");
        }

        
        [Route("dashboard/partial_views/recent-inventory", Name = "RecentInventory")]
        [HttpGet]
        public async Task<IActionResult> RecentInventory(int page)
        {
            int? id = userId;
            var model = await _getHistory.Pagination(id, page);

            ViewBag.Username = _getClaims.GetUsernameClaim(User);

            return PartialView("~/Views/Users/PartialViews/Dashboard/RecentInventory.cshtml", model);
        }


        [Route("dashboard/partial_views/timestamp", Name = "TimeStamp")]
        [HttpGet]
        public async Task<IActionResult> TimeStamp(int itemid)
        {
            var timestamp = await _getHistory.GetTimeStampByItemId(itemid);
     
            return Ok(timestamp);
        }

        [Route("dashboard/partial_views/recent-inventory-pagination", Name = "RecentInventoryPagination")]
        [HttpGet]
        public async Task<IActionResult> RecentInventoryPagination(int page)
        {
            int? id = userId;
            var model = await _getHistory.Pagination(id, page);

            return PartialView("~/Views/Users/PartialViews/Dashboard/RecentInventoryPagination.cshtml", model);
        }





        //
        //
        //Partials Views for Inventory Update, ViewDetails, Create
        //
        //
        

        [Route("inventory/update/partial_views/with_firmwareUpdate")]
        [HttpGet]
        public IActionResult HasFirmwareUpdate()
        {       
            return PartialView("~/Views/Users/PartialViews/Inventory/HasFirmwareUpdate.cshtml");
        }

        [Route("inventory/update/partial_views/no_firmwareUpdate")]
        [HttpGet]
        public IActionResult NoFirmwareUpdate()
        {
            var item = new Item();
            return PartialView("~/Views/Users/PartialViews/Inventory/NoFirmwareUpdate.cshtml", item);
        }

        [Route("inventory/details/partial_views/has_firmwareview")]
        [HttpGet]
        public async Task<IActionResult> HasFirmwareView(int? id)
        {
            var item = await _query.FindItemAsync(id);
            ViewBag.Category = item!.Category;
            return PartialView("~/Views/Users/PartialViews/Inventory/HasFirmwareView.cshtml", item);
        }

        [Route("inventory/details/partial_views/no_firmwareview")]
        [HttpGet]
        public async Task<IActionResult> NoFirmwareView(int? id)
        {
            
            var item = await _query.FindItemAsync(id);

            ViewBag.Category = item!.Category;

            return PartialView("~/Views/Users/PartialViews/Inventory/NoFirmwareView.cshtml", item);
        }

        [Route("inventory/delete/partial_views/has_firmwaredelete")]
        [HttpGet]
        public IActionResult HasFirmwareDelete()
        {
            var item = new Item();
            return PartialView("~/Views/Users/PartialViews/Inventory/HasFirmwareDelete.cshtml", item);
        }

        [Route("inventory/delete/partial_views/no_firmwaredelete")]
        [HttpGet]
        public IActionResult NoFirmwareDelete()
        {         
            var item = new Item();
            return PartialView("~/Views/Users/PartialViews/Inventory/NoFirmwareDelete.cshtml", item);
        }
    }
}
