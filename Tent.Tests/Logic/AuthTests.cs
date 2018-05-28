using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;
using Tent.Auth;
using Tent.Logic;
using static Tent.Table;

namespace Tent.Tests.Logic
{
    [TestClass]
    public class AuthTests : BaseTests
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
        public void SignUpAndLogin() {
            createUserTable();

            string email = "case@sparkle.stream";
            string password = "abcd1234";
            var signUpResult = new SignUp(email, password).Execute();
            Assert.IsTrue(!signUpResult.Failed);
        
            var loginResult = new Login(email, password).Execute();
            Assert.IsTrue(!loginResult.Failed);
        }

        [TestMethod]
        public void LoginFail() {
            string email = "case@sparkle.stream";
            string password = "oops1ts3rong";
            var result = new Login(email, password).Execute();
            Assert.IsTrue(result.Failed);
        }

        // todo: have duplicate .sql file that could be deleted now that table is created in code
        void createUserTable() {
            var sql = new Table("User")
                .AddColumn("Id", SqlType.Int, Syntax.PrimaryKey + Syntax.Identity(1, 1))
                .AddColumn("Email", SqlType.VarChar(100), Syntax.NotNull)
                .AddColumn("PasswordHash", SqlType.VarChar(255), Syntax.NotNull)
                .AddColumn("Salt", SqlType.VarChar(255), Syntax.NotNull)
                .AddColumn("DateCreated", SqlType.DateTime, Syntax.NotNull + Syntax.DefaultGetDate)
                .End()
                .Sql;
            db.Execute(sql);
        }
    }
}
