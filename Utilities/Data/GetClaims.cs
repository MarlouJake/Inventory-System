using System.Security.Claims;

namespace InventorySystem.Utilities.Data
{
    public class GetClaims
    {
        public int? GetIdClaim(ClaimsPrincipal user)
        {
            var claim = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            if (claim != null && int.TryParse(claim.Value, out int userId))
            {
                return userId;
            }

            return null;
        }

        public string? GetUsernameClaim(ClaimsPrincipal user)
        {
            var claim = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);

            return claim?.Value;
        }
    }
}
