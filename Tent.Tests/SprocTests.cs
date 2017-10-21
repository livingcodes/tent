using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Tent.Data;

namespace Tent.Tests
{
    [TestClass]
    public class SprocTests : BaseTests
    {
        public SprocTests() : base() {
            var sql = @"
                IF EXISTS (
                    SELECT * FROM INFORMATION_SCHEMA.TABLES
                    WHERE TABLE_NAME = 'Posts'
                )
                    DROP TABLE Posts
                CREATE TABLE Posts (
	                Id INT PRIMARY KEY IDENTITY(1, 1),
	                Title VARCHAR(100) NOT NULL,
	                Html VARCHAR(MAX) NOT NULL,
	                DateCreated DATETIME NOT NULL
                )";
            db.Select<int>(sql);

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
}
