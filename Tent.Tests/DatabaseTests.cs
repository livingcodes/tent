using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using Tent.Data;

namespace Tent.Tests
{
    [TestClass]
    public class ClassAndColumnsMatch : BaseTests
    {
        [TestMethod]
        public void InsertQueryUpdateDelete() {
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

    // Summary property does not have column
    // so it will be ignored
    [TestClass]
    public class PropertyWithoutColumn : BaseTests
    {
        [TestMethod]
        public void InsertAndSelect() {
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
                Title = "abc", Html = "def", Summary = "has no column"
            });
            //db.Select<int>("INSERT INTO Posts (Title, Html, DateCreated) VALUES ('abc', 'def', GETDATE())");
            var post = db.Select<Post>("SELECT * FROM Posts WHERE Title = 'abc'").First();
            Assert.IsTrue(post != null);
            Assert.IsTrue(post.Title == "abc");
            Assert.IsTrue(post.Summary == null);
        }

        class Post
        {
            public Post() {
                DateCreated = DateTime.Now;
            }
            public int Id { get; set; }
            public string Title { get; set; }
            public string Summary { get; set; }
            public string Html { get; set; }
            public DateTime DateCreated { get; set; }
        }
    }

    // CoverImage column does not have property
    //
    [TestClass]
    public class ColumnWithoutProperty : BaseTests
    {
        [TestMethod, ExpectedException(typeof(System.Data.SqlClient.SqlException))]
        public void ColumnNotNullThenThrowsException() {
            var sql = @"
                IF EXISTS (
                    SELECT * FROM INFORMATION_SCHEMA.TABLES
                    WHERE TABLE_NAME = 'Posts'
                )
                    DROP TABLE Posts
                CREATE TABLE Posts (
	                Id INT PRIMARY KEY IDENTITY(1, 1),
	                CoverImage VARCHAR(100) NOT NULL,
	                Title VARCHAR(100) NOT NULL,
	                Html VARCHAR(MAX) NOT NULL,
	                DateCreated DATETIME NOT NULL
                )";
            db.Select<int>(sql);

            db.Insert(new Post() {
                Title = "abc", Html = "def"
            });
        }

        [TestMethod]
        public void NullableColumn() {
            var sql = @"
                IF EXISTS (
                    SELECT * FROM INFORMATION_SCHEMA.TABLES
                    WHERE TABLE_NAME = 'Posts'
                )
                    DROP TABLE Posts
                CREATE TABLE Posts (
	                Id INT PRIMARY KEY IDENTITY(1, 1),
	                CoverImage VARCHAR(100),
	                Title VARCHAR(100) NOT NULL,
	                Html VARCHAR(MAX) NOT NULL,
	                DateCreated DATETIME NOT NULL
                )";
            db.Select<int>(sql);

            var rowsAffected = db.Insert(new Post() {
                Title = "abc", Html = "def"
            });

            Assert.IsTrue(rowsAffected == 1);
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
