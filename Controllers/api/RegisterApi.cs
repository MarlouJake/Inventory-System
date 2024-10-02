using InventorySystem.Data;
using InventorySystem.Models.DataEntities;
using InventorySystem.Models.Responses;
using InventorySystem.Utilities.Api;
using InventorySystem.Utilities.Data;
using Microsoft.AspNetCore.Mvc;

namespace InventorySystem.Controllers.api
{
    [ApiController]
    [Route("api/r/")]
    public class RegisterApi(ApplicationDbContext context) : ControllerBase
    {
        private readonly ApplicationDbContext _context = context;
        public ApiMessageListResponse? responseList;

        [HttpPost("create/user")]
        public async Task<IActionResult> CreateNewAccount([FromBody] User model)
        {
            var newUser = new User
            {
                Username = model.Username,
                Email = model.Email,
                Password = HashHelper.HashString(model.Password!),
            };

            var seedrole = new SeedUserRole(_context);
            var (isSuccess, seedMessage, statuscode) = await seedrole.AddUserWithRole(newUser, "User");
            /*
            var responseObject = new ApiMessageListResponse
            {
                IsValid = true,
                Message = seedMessage,
                Model = null,
                RedirectUrl = null
            };*/

            if (isSuccess)
            {
                responseList = ApiResponseList.MessageListResponse(true, seedMessage);
                Console.WriteLine("Registration Successful");
                return StatusCode(statuscode, responseList);
            }
            else
            {
                responseList = ApiResponseList.MessageListResponse(false, seedMessage);
                Console.WriteLine("Registration Unsuccessful");
                return StatusCode(statuscode, responseList);
            }
        }
    }
}
