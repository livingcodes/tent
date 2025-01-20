namespace Tent.Auth;
using System.Security.Cryptography;
public class Salt
{
  public Salt(int size = 16) {
    AsByteArray = gen(size);
  }

  public byte[] AsByteArray { get; }

  public str AsString => asString 
    ?? (asString = Convert.ToBase64String(AsByteArray));
  str asString;

  public override string ToString() => AsString;

  byte[] gen(int size) {
    byte[] salt = new byte[size];
    using (var rng = RandomNumberGenerator.Create())
      rng.GetBytes(salt);
    return salt;
  }
}