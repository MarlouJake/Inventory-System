using InventorySystem.Data;
using InventorySystem.Models.DataEntities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace InventorySystem.Controllers.api
{
    [ApiController]
    [Route("api/a/")]
    public class ActionLogger(ApplicationDbContext context) : ControllerBase
    {
        private readonly ApplicationDbContext _context = context;

        [Route("log-data")]
        public async Task<IActionResult> SaveLoginData([FromBody] string jsonData)
        {
            var data = JsonConvert.DeserializeObject<LoginData>(jsonData);
            _context.Add(jsonData);
            await _context.SaveChangesAsync();
            return StatusCode(StatusCodes.Status201Created);
        }
    }
}
