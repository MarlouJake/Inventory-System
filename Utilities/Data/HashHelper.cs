using System.Security.Cryptography;
using System.Text;

namespace InventorySystem.Utilities.Data
{
    /// <summary>
    /// Provides utility methods for hashing passwords and IDs.
    /// </summary>
    public class HashHelper
    {
        /// <summary>
        /// Hashes a password using SHA-256.
        /// </summary>
        /// <param name="password">The password to hash.</param>
        /// <returns>A hexadecimal string representation of the hashed password.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the password is null.</exception>
        public static string HashString(string password)
        {
            if (password == null)
            {
                throw new ArgumentNullException(nameof(password), "Password cannot be null");
            }

            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
            var builder = new StringBuilder();
            foreach (var bit in bytes)
            {
                builder.Append(bit.ToString("x2"));
            }
            return builder.ToString();
        }

        /// <summary>
        /// Hashes an integer ID using Base64 encoding.
        /// </summary>
        /// <param name="id">The ID to hash. Must have a value.</param>
        /// <returns>A Base64 encoded string representation of the hashed ID.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the ID is null.</exception>
        public static string Hash(int? id)
        {
            if (!id.HasValue)
            {
                throw new ArgumentNullException(nameof(id), "ID cannot be null");
            }

            return Convert.ToBase64String(BitConverter.GetBytes(id.Value));
        }

        /// <summary>
        /// Converts a Base64 encoded string back to an integer ID.
        /// </summary>
        /// <param name="hashedId">The Base64 encoded string to unhash.</param>
        /// <returns>The integer ID represented by the Base64 encoded string.</returns>
        /// <exception cref="FormatException">Thrown when the Base64 string is not valid.</exception>
        public static int Unhash(string hashedId)
        {
            if (string.IsNullOrEmpty(hashedId))
            {
                throw new ArgumentNullException(nameof(hashedId), "Hashed ID cannot be null or empty");
            }

            var bytes = Convert.FromBase64String(hashedId);
            if (bytes.Length != 4)
            {
                throw new FormatException("The hashed ID is not in the correct format.");
            }
            return BitConverter.ToInt32(bytes, 0);
        }
    }
}
