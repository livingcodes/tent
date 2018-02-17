using System;
using System.Security.Cryptography;

namespace Tent.Auth
{
    public class Salt
    {
        public Salt(int size = 16) {
            AsByteArray = generate(size);
        }
        
        public byte[] AsByteArray { get; }

        public string AsString => asString 
            ?? (asString = Convert.ToBase64String(AsByteArray));
        string asString;

        public override string ToString() => AsString;

        byte[] generate(int size) {
            byte[] salt = new byte[size];
            using (var rng = RandomNumberGenerator.Create())
                rng.GetBytes(salt);
            return salt;
        }
    }
}