using System;
using System.Linq;
using System.Security.Cryptography;

namespace YourNamespace.Utilities
{
    public static class HashPassword
    {
        public static string Hash(string password)
        {
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);

            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000))
            {
                byte[] hash = pbkdf2.GetBytes(20);
                byte[] hashBytes = new byte[36];
                Array.Copy(salt, 0, hashBytes, 0, 16);
                Array.Copy(hash, 0, hashBytes, 16, 20);

                return Convert.ToBase64String(hashBytes);
            }
        }

        public static bool Verify(string password, string storedHash)
        {
            byte[] hashBytes = Convert.FromBase64String(storedHash);
            byte[] salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);

            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000))
            {
                byte[] hash = pbkdf2.GetBytes(20);
                byte[] storedHashBytes = new byte[20];
                Array.Copy(hashBytes, 16, storedHashBytes, 0, 20);

                return hash.SequenceEqual(storedHashBytes);
            }
        }
    }
}
