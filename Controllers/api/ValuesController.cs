using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InventorySystem.Controllers.api
{
    [Route("api/values/")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        [Route("get-statuses")]
        [HttpGet]
        public JsonResult GetStatuses()
        {
            var statuses = new List<SelectListItem>
            {
                //new() { Value = "--Select Status--", Text = "--Select Status--" },
                new() { Value = "Complete", Text = "Complete" },
                new() { Value = "Incomplete(Usable)", Text = "Incomplete(Usable)" },
                new() { Value = "Incomplete(Unusable)", Text = "Incomplete(Unusable)" }
            };

            return new JsonResult(statuses);
        }

        [Route("get-options")]
        [HttpGet]
        public JsonResult GetOptions()
        {
            var options = new List<SelectListItem>
            {
                //new() { Value = "--Select Status--", Text = "--Select Status--" },
                new() { Value = "N/A", Text = "N/A" },
                new() { Value = "Yes", Text = "Yes" },
                new() { Value = "No", Text = "No" }
            };

            return new JsonResult(options);
        }
    }
}
