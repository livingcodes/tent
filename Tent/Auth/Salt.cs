using System;
using System.Security.Cryptography;
using System.Text;

namespace Tent.Auth
{
    public class Salt
    {
        public Salt(int size = 16) {
            AsByteArray = generate(size);
        }
        
        public byte[] AsByteArray { get; }
        public string AsString { get {
            if (asString == null)
                asString = Convert.ToBase64String(AsByteArray);
            return asString;
        } }
        string asString;

        byte[] generate(int size) {
            byte[] salt = new byte[size];
            using (var rng = RandomNumberGenerator.Create())
                rng.GetBytes(salt);
            return salt;
        }
    }
}
