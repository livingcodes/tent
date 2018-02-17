namespace Tent.Auth
{
    using Microsoft.AspNetCore.Cryptography.KeyDerivation;
    using System;

    public class Hash
    {
        public Hash(string password, byte[] salt) {
            this.password = password;
            this.salt = salt;
            AsByteArray = generate(password, salt);
        }

        public Hash(string password, string salt) {
            this.password = password;
            this.salt = Convert.FromBase64String(salt);
            AsByteArray = generate(password, this.salt);
        }

        public byte[] AsByteArray { get; }

        public string AsString => asString 
            ?? (asString = Convert.ToBase64String(AsByteArray));
        string asString;

        public override string ToString() => AsString;

        string password;
        byte[] salt;

        byte[] generate(string password, byte[] salt) {
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