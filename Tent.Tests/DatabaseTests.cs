using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
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
            var posts = db.Query<Post>("select * from posts");
            Assert.IsTrue(posts.Count > 0);
        }

        [TestMethod]
        public void Insert() {
            db.Insert<Post>(new Post() {
                Title = "ASP.NET Core DI",
                Html = "<h1>ASP.NET Core DI</h1><p>Dependency injection</p>"
            });
        }
    }
    class User
    {
        public string Id { get; set; }
        public string Email { get; set; }
    }
    class Post
    {
        public Post() {
            DateCreated = DateTime.Now;
        }
        public int Id { get; set; }
        public string Title { get; set; }
        public string Html { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
