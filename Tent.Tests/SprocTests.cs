using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Tent.Data;

namespace Tent.Tests
{
    [TestClass]
    public class SprocTests : BaseTests
    {
        public SprocTests() : base() {
            db.Truncate<Post>();
            db.Insert(new Post() {
                Title = "A", Html = "B"
            });
        }

        [TestMethod]
        public void QuerySproc() {
            var posts = db.Sproc("GetPosts").Select<Post>();
            Assert.IsTrue(posts.Count > 0);
        }

        [TestMethod]
        public void QuerySprocWithParameter() {
            var posts = db.Sproc("GetPostsByDateCreated")
                .Parameter("@DateCreated", DateTime.Now.AddDays(-10))
                .Select<Post>();
            Assert.IsTrue(posts.Count > 0);
        }

        [TestMethod]
        public void QuerySprocWithParameterReturnSingle() {
            var post = db.Sproc("GetPostById")
                .Parameter("@Id", 1)
                .SelectOne<Post>();
            Assert.IsTrue(post.Id >= 1);
        }
    }
}
