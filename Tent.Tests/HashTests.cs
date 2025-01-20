namespace Tent.Tests;
using Tent.Auth;
[TestClass]
public class HashTests:BaseTests
{
  [TestMethod]
  public void HashStringsMatch() {
    string password = "password";
    var salt = new Salt();
    var hash1 = new Hash(password, salt.AsByteArray).AsString;
    var hash2 = new Hash(password, salt.AsByteArray).AsString;
    assert(hash1 == hash2);
  }
}