using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace Tent.Tests
{
    [TestClass]
    public class DatabaseTests
    {
        [TestMethod]
        public void QueryTable() {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(@"c:\code\tent\tent.tests\appsettings.json")
                .Build();
            string connectionString = config["connectionString"];

            IDatabase db = new Database(connectionString);
            var users = db.Query<User>("select * from aspnetusers");
            Assert.IsTrue(users.Count > 0);
        }
    }
    class User
    {
        public string Id { get; set; }
        public string Email { get; set; }
    }
}
