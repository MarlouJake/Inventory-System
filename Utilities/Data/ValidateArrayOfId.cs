using InventorySystem.Data;
using InventorySystem.Models.Responses;
using InventorySystem.Utilities.Api;
using Microsoft.EntityFrameworkCore;

namespace InventorySystem.Utilities.Data
{
    public class ValidateArrayOfId
    {
        public async Task<ApiResponse> ValidateAsync(string[] ids, ApplicationDbContext _context)
        {
            string message;

            if (ids == null || ids.Length == 0)
            {
                message = "No IDs provided for deletion.";
                return await Task.FromResult(ApiResponseUtils.CustomResponse(false, message, null));
            }

            List<Guid> invalidIds = new List<Guid>();

            foreach (var id in ids)
            {
                Guid.TryParse(id, out var parsedId);
                var item = await _context.Items.FirstOrDefaultAsync(model => model.UniqueId == parsedId);

                if (item == null)
                {
                    invalidIds.Add(parsedId);
                    Console.WriteLine($"Item with ID {id} doesn't exist");
                    continue;
                }

                Console.WriteLine("Selected id: {0}", id);
            }

            if (invalidIds.Any())
            {
                message = $"The following IDs don't exist: {string.Join(", ", invalidIds)}";
                return await Task.FromResult(ApiResponseUtils.CustomResponse(false, message, null));
            }

            message = "All IDs are valid.";
            return await Task.FromResult(ApiResponseUtils.CustomResponse(true, message, null));
        }
    }
}
