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
            var post = new Post() {
                Title = "ASP.NET Core DI",
                Html = "<h1>ASP.NET Core DI</h1><p>Dependency injection</p>"
            };
            db.Insert<Post>(post);
            var posts = db.Query<Post>($"select * from posts where title = '{post.Title}'");
            Assert.IsTrue(posts.Count > 0);

            db.Delete<Post>(post.Id);
        }

        [TestMethod]
        public void Delete() {
            var post = new Post() {
                Title = "ASP.NET Core DI",
                Html = "<h1>ASP.NET Core DI</h1><p>Dependency injection</p>"
            };
            db.Insert<Post>(post);
            var posts = db.Query<Post>($"select * from posts where title = '{post.Title}'");
            Assert.IsTrue(posts.Count > 0);

            foreach (var p in posts)
                db.Delete<Post>(p.Id);

            posts = db.Query<Post>($"select * from posts where title = '{post.Title}'");
            Assert.IsTrue(posts.Count == 0);
        }

        [TestMethod]
        public void QueryWithParameter() {
            throw new NotImplementedException(nameof(QueryWithParameter));
        }

        [TestMethod]
        public void HandleNullDateTime() {
            
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
