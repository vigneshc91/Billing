using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace BillingSoftware.Helper
{
    public class PasswordHash
    {
        // The following constants may be changed without breaking existing hashes.
        public const int SALT_BYTE_SIZE = 24;
        public const int HASH_BYTE_SIZE = 24;
        public const int PBKDF2_ITERATIONS = 1000;

        public const int ITERATION_INDEX = 0;
        public const int SALT_INDEX = 1;
        public const int PBKDF2_INDEX = 2;

        public const int PASSWORD_LENGTH = 8;

        /// <summary>
        /// Creates a salted PBKDF2 hash of the password.
        /// </summary>
        /// <param name="password">The password to hash.</param>
        /// <returns>The hash of the password.</returns>
        public static string CreateHash(string password, string salt)
        {
            // Generate a random salt
            //          RNGCryptoServiceProvider csprng = new RNGCryptoServiceProvider ();
            //          byte[] salt = new byte[SALT_BYTE_SIZE];
            //          csprng.GetBytes (salt);

            // Hash the password and encode the parameters
            byte[] saltByte = Convert.FromBase64String(salt);
            byte[] hash = PBKDF2(password, saltByte, HASH_BYTE_SIZE);
            string hashString = Convert.ToBase64String(hash);
            return hashString;
        }

        /// <summary>
        /// Validates a password given a hash of the correct one.
        /// </summary>
        /// <param name="password">The password to check.</param>
        /// <param name="correctHash">A hash of the correct password.</param>
        /// <returns>True if the password is correct. False otherwise.</returns>
        public static bool ValidatePassword(string password, string correctHash, string saltString)
        {
            // Extract the parameters from the hash
            //          char[] delimiter = { ':' };
            //          string[] split = correctHash.Split (delimiter);
            //          int iterations = Int32.Parse (split[ITERATION_INDEX]);
            byte[] salt = Convert.FromBase64String(saltString);
            byte[] hash = Convert.FromBase64String(correctHash);

            byte[] testHash = PBKDF2(password, salt, hash.Length);
            return SlowEquals(hash, testHash);
        }

        /// <summary>
        /// Compares two byte arrays in length-constant time. This comparison
        /// method is used so that password hashes cannot be extracted from
        /// on-line systems using a timing attack and then attacked off-line.
        /// </summary>
        /// <param name="a">The first byte array.</param>
        /// <param name="b">The second byte array.</param>
        /// <returns>True if both byte arrays are equal. False otherwise.</returns>
        private static bool SlowEquals(byte[] a, byte[] b)
        {
            uint diff = (uint)a.Length ^ (uint)b.Length;
            for (int i = 0; i < a.Length && i < b.Length; i++) diff |= (uint)(a[i] ^ b[i]);
            return diff == 0;
        }

        /// <summary>
        /// Computes the PBKDF2-SHA1 hash of a password.
        /// </summary>
        /// <param name="password">The password to hash.</param>
        /// <param name="salt">The salt.</param>
        /// <param name="iterations">The PBKDF2 iteration count.</param>
        /// <param name="outputBytes">The length of the hash to generate, in bytes.</param>
        /// <returns>A hash of the password.</returns>
        private static byte[] PBKDF2(string password, byte[] salt, int outputBytes)
        {
            Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(password, salt, PBKDF2_ITERATIONS);
            return pbkdf2.GetBytes(outputBytes);
        }


        /// <summary>
        /// Generates the salt.
        /// </summary>
        /// <returns>The salt.</returns>
        public static string GenerateSalt()
        {
            RNGCryptoServiceProvider csprng = new RNGCryptoServiceProvider();
            byte[] salt = new byte[SALT_BYTE_SIZE];
            csprng.GetBytes(salt);
            string saltString = Convert.ToBase64String(salt);
            return saltString;
        }

        public static string GenerateToken(string a)
        {

            char[] chars;
            chars = a.ToCharArray();
            byte[] data = new byte[PASSWORD_LENGTH];
            RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider();
            crypto.GetNonZeroBytes(data);
            StringBuilder result = new StringBuilder(PASSWORD_LENGTH);
            foreach (byte b in data)
            {
                result.Append(chars[b % chars.Length]);
            }
            return result.ToString();
        }



    }
}