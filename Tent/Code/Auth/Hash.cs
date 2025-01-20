namespace Tent.Auth;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
public class Hash
{
  public Hash(str pw, byte[] salt) {
    this.pw = pw;
    this.salt = salt;
    AsByteArray = gen(pw, salt);
  }

  public Hash(str pw, str salt) {
    this.pw = pw;
    this.salt = Convert.FromBase64String(salt);
    AsByteArray = gen(pw, this.salt);
  }

  str pw;
  byte[] salt;

  public byte[] AsByteArray { get; }

  public str AsString => asString 
    ?? (asString = Convert.ToBase64String(AsByteArray));
  str asString;

  public override string ToString() => AsString;

  byte[] gen(str pw, byte[] salt) {
    byte[] hash = KeyDerivation.Pbkdf2(
      pw,
      salt,
      KeyDerivationPrf.HMACSHA256,
      iterationCount: 10000,
      numBytesRequested: 256 / 8);
    return hash;
  }
}