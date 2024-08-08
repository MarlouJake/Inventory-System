using System.Security.Cryptography;
using System.Text;

namespace InventorySystem.Utilities
{
    public class HashHelper
    {
        public static string HashPassword(string password)
        {
            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
            var builder = new StringBuilder();
            foreach (var bit in bytes)
            {
                builder.Append(bit.ToString("x2"));
            }
            return builder.ToString();
        }

        public static string Hash(int? id)
        {
            if (!id.HasValue)
            {
                throw new ArgumentNullException(nameof(id), "ID cannot be null");
            }

            return Convert.ToBase64String(BitConverter.GetBytes(id.Value));
        }

        public static int Unhash(string hashedId)
        {
            // Your logic to unhash the ID
            var bytes = Convert.FromBase64String(hashedId);
            return BitConverter.ToInt32(bytes, 0);
        }
    }
}
