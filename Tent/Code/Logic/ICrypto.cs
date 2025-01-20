namespace Tent.Logic;
using Microsoft.AspNetCore.DataProtection;

public interface ICrypto
{
  str Encrypt(str val);
  str Decrypt(str val);
}

public class Crypto : ICrypto
{
  // note: encrypted value expires and then can't be decrypted
  public Crypto(IDataProtectionProvider provider) =>
    protector = provider.CreateProtector("encrypt-cookie");
  IDataProtector protector;

  public str Encrypt(str val) =>
    protector.Protect(val);

  public str Decrypt(str val) =>
    protector.Unprotect(val);
}