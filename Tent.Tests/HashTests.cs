namespace Tent.Tests;
using Tent.Auth;
[tc]public class HashTests:BaseTests
{
  [tm]public void HashStringsMatch() {
    str pw = "password";
    var salt = new Salt();
    var hash1 = new Hash(pw, salt.AsByteArray).AsString;
    var hash2 = new Hash(pw, salt.AsByteArray).AsString;
    assert(hash1 == hash2);
  }
}