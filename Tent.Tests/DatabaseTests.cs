using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace Tent.Tests
{
    public class BaseTests
    {
        public BaseTests() {
            this.configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(@"c:\code\secrets\Tent\settings.json")
                .Build();
            string connectionString = configuration["connectionString"];
            this.db = new Database(connectionString);
        }

        protected IConfigurationRoot configuration;
        protected IDatabase db;
    }

    [TestClass]
    public class DatabaseTests : BaseTests
    {
        [TestMethod]
        public void QueryTable() {
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
