using IdServer.Core.Services;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace IdServer.Infraestructure.Services
{
    public class PasswordHasher : IPasswordHasher
    {
        public string HashPassword(string password, KeyDerivationPrf prf, int saltSize, int numberOfIterations, int outputKeySize)
        {
            var outputBytes = new byte[saltSize + outputKeySize];
            using (var rng = RandomNumberGenerator.Create())
            {
                var salt = new byte[saltSize];
                rng.GetBytes(salt);

                var derivedKey = KeyDerivation.Pbkdf2(password, salt, prf, numberOfIterations, outputKeySize);

                Buffer.BlockCopy(salt, 0, outputBytes, 0, saltSize);
                Buffer.BlockCopy(derivedKey, 0, outputBytes, saltSize, outputKeySize);

                return Convert.ToBase64String(outputBytes);
            }
        }

        public bool VerifyHashedPassword(string hashedPassword, string providedPassword, KeyDerivationPrf prf, int saltSize, int numberOfIterations, int outputKeySize)
        {
            if (hashedPassword == null)
            {
                throw new ArgumentNullException(nameof(hashedPassword));
            }
            if (providedPassword == null)
            {
                throw new ArgumentNullException(nameof(providedPassword));
            }

            byte[] decodedHashedPassword = Convert.FromBase64String(hashedPassword);

            if (decodedHashedPassword.Length == 0)
            {
                return false;
            }
            return VerifyHashedPassword(decodedHashedPassword, providedPassword, prf, saltSize, numberOfIterations, outputKeySize);
        }

        private static bool VerifyHashedPassword(byte[] hashedPassword, string password, KeyDerivationPrf prf, int saltSize, int numberOfIterations, int outputKeySize)
        {
            // We know ahead of time the exact length of a valid hashed password payload.
            if (hashedPassword.Length != saltSize + outputKeySize)
            {
                return false; // bad size
            }

            byte[] salt = new byte[saltSize];
            Buffer.BlockCopy(hashedPassword, 0, salt, 0, salt.Length);

            byte[] expectedSubkey = new byte[outputKeySize];
            Buffer.BlockCopy(hashedPassword, salt.Length, expectedSubkey, 0, expectedSubkey.Length);

            // Hash the incoming password and verify it
            byte[] actualSubkey = KeyDerivation.Pbkdf2(password, salt, prf, numberOfIterations, outputKeySize);
            return ByteArraysEqual(actualSubkey, expectedSubkey);
        }

        private static bool ByteArraysEqual(byte[] a, byte[] b)
        {
            if (a == null && b == null)
            {
                return true;
            }
            if (a == null || b == null || a.Length != b.Length)
            {
                return false;
            }
            var areSame = true;
            for (var i = 0; i < a.Length; i++)
            {
                areSame &= (a[i] == b[i]);
            }
            return areSame;
        }
    }
}
