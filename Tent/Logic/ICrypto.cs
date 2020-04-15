using Microsoft.AspNetCore.DataProtection;

namespace Tent.Logic
{
    public interface ICrypto
    {
        string Encrypt(string value);
        string Decrypt(string value);
    }

    public class Crypto : ICrypto
    {
        // note: encrypted value expires and then can't be decrypted
        public Crypto(IDataProtectionProvider provider) =>
            protector = provider.CreateProtector("encrypt-cookie");
        
        IDataProtector protector;

        public string Encrypt(string value) =>
            protector.Protect(value);

        public string Decrypt(string value) =>
            protector.Unprotect(value);
    }
}