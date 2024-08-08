using System.Security.Cryptography;
using System.Text;

namespace InventorySystem.Utilities
{
    public class IdHasher
    {
        //Secret Key = c8539f586dfcc187258dfeec07d95c665adec396f0c9824426561fbe6c24b498
        public static readonly string SecretKey = "c8539f586dfcc187258dfeec07d95c665adec396f0c9824426561fbe6c24b498";

        public static string HashId(int id)
        {
            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(SecretKey));
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(id.ToString()));
            return Convert.ToBase64String(hash);

        }

        public static int UnhashId(string hash)
        {
            throw new NotImplementedException("Unhashing is complex and often not feasible. Consider using a reversible encryption algorithm instead.");
        }
    }
}
