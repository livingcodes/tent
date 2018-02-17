namespace Tent.Auth
{
    using Microsoft.AspNetCore.Cryptography.KeyDerivation;
    using System;
    using System.Text;

    public class Hash
    {
        public Hash(string password, byte[] salt) {
            this.password = password;
            this.salt = salt;
        }

        public Hash(string password, string salt) {
            this.password = password;
            this.salt = Convert.FromBase64String(salt);
        }

        string password;
        byte[] salt;

        /// <summary>Generates hash</summary>
        public string Generate() {
            var hash = generateBytes();
            var hashString = Convert.ToBase64String(hash);
            return hashString;
        }

        byte[] generateBytes() {
            byte[] hash = KeyDerivation.Pbkdf2(
                password,
                salt,
                KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8);
            return hash;
        }
    }
}