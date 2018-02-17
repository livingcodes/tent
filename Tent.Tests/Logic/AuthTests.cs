using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;
using Tent.Auth;
using Tent.Logic;

namespace Tent.Tests.Logic
{
    [TestClass]
    public class AuthTests
    {
        [TestMethod]
        public void SaltAs() {
            var salt = new Salt();
            var saltAsString = salt.AsString;
            var saltAsByteArray = Encoding.UTF8.GetBytes(saltAsString, 0, saltAsString.Length);
            var saltAsString2 = Encoding.UTF8.GetString(saltAsByteArray, 0, saltAsByteArray.Length);
            Assert.IsTrue(saltAsString == saltAsString2);
        }

        [TestMethod]
        public void SignUp() {
            string email = "case@sparkle.stream";
            string password = "abcd1234";
            var result = new SignUp(email, password).Execute();
            Assert.IsTrue(!result.Failed);
        }

        [TestMethod]
        public void Login() {
            string email = "case@sparkle.stream";
            string password = "abcd1234";
            var result = new Login(email, password).Execute();
            Assert.IsTrue(!result.Failed);
        }

        [TestMethod]
        public void LoginFail() {
            string email = "case@sparkle.stream";
            string password = "oops1ts3rong";
            var result = new Login(email, password).Execute();
            Assert.IsTrue(result.Failed);
        }
    }
}
