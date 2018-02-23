using Microsoft.AspNetCore.DataProtection;

namespace Tent.Logic
{
    public interface ICryptographer
    {
        string Encrypt(string value);
        string Decrypt(string value);
    }

    public class Cryptographer : ICryptographer
    {
        public Cryptographer(IDataProtectionProvider provider) =>
            protector = provider.CreateProtector("encrypt-cookie");
        
        IDataProtector protector;

        public string Encrypt(string value) =>
            protector.Protect(value);

        public string Decrypt(string value) =>
            protector.Unprotect(value);
    }
}