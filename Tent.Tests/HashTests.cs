﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tent.Auth;
namespace Tent.Tests;
[TestClass]
public class HashTests
{
   [TestMethod]
   public void HashStringsMatch() {
      string password = "password";
      var salt = new Salt();
      var hash1 = new Hash(password, salt.AsByteArray).AsString;
      var hash2 = new Hash(password, salt.AsByteArray).AsString;
      Assert.IsTrue(hash1 == hash2);
   }
}