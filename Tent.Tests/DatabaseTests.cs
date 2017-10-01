using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Tent.Data;

namespace Tent.Tests
{
    [TestClass]
    public class DatabaseTests : BaseTests
    {
        [TestMethod]
        public void InsertQueryUpdateDelete() {
            db.Truncate<Post>();

            // insert
            var post = new Post() {
                Title = "ASP.NET Core DI",
                Html = "<h1>ASP.NET Core DI</h1><p>Dependency injection</p>"
            };
            db.Insert(post);

            // verify insert (query sql)
            var posts = db.Select<Post>("select * from posts");
            Assert.IsTrue(posts.Count > 0);

            // update title
            post = posts[0];
            post.Title = "Updated";
            db.Update(post);

            // verify update
            post = db.Select<Post>(post.Id);
            Assert.IsTrue(post.Title == "Updated");

            // delete
            db.Delete<Post>(post.Id);

            // verify delete (query id)
            post = db.Select<Post>(post.Id);
            Assert.IsTrue(post == null);
        }
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
