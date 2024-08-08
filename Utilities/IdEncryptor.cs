using System.Security.Cryptography;


namespace InventorySystem.Utilities
{
    public class IdEncryptor
    {
        private static readonly int KeySize = 32; // 256 bits
        private static readonly int IvSize = 16;  // 128 bits

        public static string EncryptId(int id, out byte[] key, out byte[] iv)
        {
            using var aes = Aes.Create();
            aes.KeySize = KeySize * 8;  // AES key size in bits
            aes.BlockSize = IvSize * 8; // AES block size in bits
            aes.GenerateKey();
            aes.GenerateIV();

            key = aes.Key;
            iv = aes.IV;

            using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            using var ms = new MemoryStream();
            using var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write);
            using var sw = new StreamWriter(cs);
            sw.Write(id);
            sw.Close();
            return Convert.ToBase64String(ms.ToArray());
        }

        public static int DecryptId(string encryptedId, byte[] key, byte[] iv)
        {
            using var aes = Aes.Create();
            aes.Key = key;
            aes.IV = iv;

            using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            using var ms = new MemoryStream(Convert.FromBase64String(encryptedId));
            using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
            using var sr = new StreamReader(cs);
            return int.Parse(sr.ReadToEnd());
        }
    }

}
