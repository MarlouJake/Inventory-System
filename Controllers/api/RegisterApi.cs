using Azure;
using Google.Protobuf.WellKnownTypes;
using InventorySystem.Data;
using InventorySystem.Models.DataEntities;
using InventorySystem.Models.Responses;
using InventorySystem.Utilities.Api;
using InventorySystem.Utilities.Data;
using Microsoft.AspNetCore.Mvc;
using Mysqlx;
using System.Text.Json;

namespace InventorySystem.Controllers.api
{
    [ApiController]
    [Route("api/r/")]
    public class RegisterApi(ApplicationDbContext context) : ControllerBase
    {
        private readonly ApplicationDbContext _context = context;

        private ApiMessageListResponse? responseList;
        private readonly List<string> serverLog = [];
        private readonly ServerLog responseObject = new();
        private User newUser = new();
        private object userObj = new { };

       [HttpPost("create/user")]
        public async Task<IActionResult> CreateNewAccount([FromBody] User model)
        {          
            var seedrole = new SeedUserRole(_context);
            string encryption = $"{HttpContext.Request.Scheme}://";

            newUser.Construct(model.Username, model.Email, HashHelper.HashString(model.Password!));
        
            var (isSuccess, seedMessage, statuscode) = await seedrole.AddUserWithRole(newUser, "User");

            responseObject.Construct(isSuccess, ["ServerMessage"], model, 
                string.Join("", encryption, HttpContext.Request.Host, HttpContext.Request.Path), 
                "Create Account", string.Join(", ", seedMessage), statuscode); 
            
            UpdateServerResponse(isSuccess, seedMessage, responseObject);
                   
            return StatusCode(statuscode, responseList);
        }

      
        private void UpdateServerResponse(bool isSuccess, List<string> messages, object obj)
        {
            string response = ToJsonFormatAsync(obj);
            Console.WriteLine($"Server Log: {response}");

            switch (isSuccess)
            {
                case true:
                    responseList = ApiResponseList.MessageListResponse(true, messages);
                    serverLog.Add("Registration Successsful");
                    break;
                case false:
                    responseList = ApiResponseList.MessageListResponse(false, messages);
                    serverLog.Add("Registration Unsuccessful");
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(isSuccess),"Invalid argument value type.");
            }
  
        }


        private static string ToJsonFormatAsync(object obj)
        {
            JsonSerializerOptions options = GetSerializerOptions();
            return JsonSerializer.Serialize(obj, options);
        }


        private static JsonSerializerOptions GetSerializerOptions()
        {
            return new() 
            {
                WriteIndented = true 
            };
        }
    }
}
