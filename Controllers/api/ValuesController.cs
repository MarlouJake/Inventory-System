using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InventorySystem.Controllers.api
{
    [Route("api/values/")]
    [ApiController]
    public class ValuesController(IConfiguration configuration): ControllerBase
    {
        private readonly IConfiguration _configuration = configuration;

        [Route("get-statuses")]
        [HttpGet]
        public JsonResult GetStatuses()
        {
            var statuses = new List<SelectListItem>
            {
                new() {Value = "", Text = "--Select Option--"},
                new() { Value = "Complete", Text = "Complete" },
                new() { Value = "Incomplete(Usable)", Text = "Incomplete(Usable)" },
                new() { Value = "Incomplete(Unusable)", Text = "Incomplete(Unusable)" },
                new() {Value = "Damaged", Text = "Damaged"},
                new() {Value = "Defective", Text = "Defective"},
                new() {Value = "Missing", Text = "Missing"},
                new() {Value = "Unknown", Text = "Unknown"}
            };

            return new JsonResult(statuses);
        }

        [Route("get-condition")]
        [HttpGet]
        public JsonResult GetCondition()
        {
            var status = new List<SelectListItem>
            {
                new() {Value = "", Text = "--Select Option--"},
                new() {Value = "New", Text = "New"},
                new() {Value = "Good", Text = "Good"},
                new() {Value = "Poor", Text = "Poor"},
                new() {Value = "Unknown", Text = "Unknown"},
                new() {Value = "Damaged", Text = "Damaged"},
                new() {Value = "Missing", Text = "Missing"}
            };

            return new JsonResult(status);
        }

        [Route("get-options")]
        [HttpGet]
        public JsonResult GetOptions()
        {
            var options = new List<SelectListItem>
            {
                new() {Value = "", Text = "--Select Option--"},
                new() { Value = "Updated", Text = "Updated" },
                new() { Value = "Not Updated", Text = "Not Updated" },
                new() { Value = "Unknown", Text = "Unknown"}
            };

            return new JsonResult(options);
        }

        [Route("get-categories")]
        [HttpGet]
        public JsonResult GetCategories()
        {
            var options = new List<SelectListItem>
            {
                new() {Value = "", Text = "--Select Option--"},
                new() { Value = "Robots", Text = "Robots" },
                new() { Value = "Books", Text = "Books" },
                new() { Value = "Materials", Text = "Materials" }
            };

            return new JsonResult(options);
        }



        [HttpGet("configs")]
        public IActionResult GetCredentials()
        {
            var appsettings = _configuration.GetSection("TestCredentials").GetChildren();
            return Ok(appsettings);
        }
    }
}
