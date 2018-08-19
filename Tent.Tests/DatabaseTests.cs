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
                    WHERE TABLE_NAME = 'Post'
                )
                    DROP TABLE Post
                CREATE TABLE Post (
	                Id INT PRIMARY KEY IDENTITY(1, 1),
	                Title VARCHAR(100) NOT NULL,
	                Html VARCHAR(MAX) NOT NULL,
	                DateCreated DATETIME NOT NULL
                )";
            db.Execute(sql);

            // insert
            var post = new Post {
                Title = "ASP.NET Core DI",
                Html = "<h1>ASP.NET Core DI</h1><p>Dependency injection</p>"
            };
            db.Insert(post);

            // verify insert (query sql)
            var posts = db.Select<Post>("select * from post");
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
                    WHERE TABLE_NAME = 'Post'
                )
                    DROP TABLE Post
                CREATE TABLE Post (
	                Id INT PRIMARY KEY IDENTITY(1, 1),
	                Title VARCHAR(100) NOT NULL,
	                Html VARCHAR(MAX) NOT NULL,
	                DateCreated DATETIME NOT NULL
                )";
            db.Execute(sql);

            db.Insert(new Post {
                Title = "abc", Html = "def", Summary = "has no column"
            });
            //db.Select<int>("INSERT INTO Posts (Title, Html, DateCreated) VALUES ('abc', 'def', GETDATE())");
            var post = db.Select<Post>("SELECT * FROM Post WHERE Title = 'abc'").First();
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
    [TestClass]
    public class ColumnWithoutProperty : BaseTests
    {
        // table has column that can't be null 
        //  and it does not have a mapped property
        // when insert
        // then an exception is thrown 
        //  because a null is attempted to be inserted into a column that does not accept null
        //  there's not a way for tada to know what default value to use (if that's even desired)
        //  but the class constructor could set a default value for the property,
        //  if a default is desired
        [TestMethod, ExpectedException(typeof(System.Data.SqlClient.SqlException))]
        public void ColumnNotNullThenThrowsException() {
            var sql = @"
                IF EXISTS (
                    SELECT * FROM INFORMATION_SCHEMA.TABLES
                    WHERE TABLE_NAME = 'Post'
                )
                    DROP TABLE Post
                CREATE TABLE Post (
	                Id INT PRIMARY KEY IDENTITY(1, 1),
	                CoverImage VARCHAR(100) NOT NULL,
	                Title VARCHAR(100) NOT NULL,
	                Html VARCHAR(MAX) NOT NULL,
	                DateCreated DATETIME NOT NULL
                )";
            db.Execute(sql);

            db.Insert(new Post {
                Title = "abc", Html = "def"
            });
        }

        [TestMethod]
        public void NullableColumn() {
            var sql = @"
                IF EXISTS (
                    SELECT * FROM INFORMATION_SCHEMA.TABLES
                    WHERE TABLE_NAME = 'Post'
                )
                    DROP TABLE Post
                CREATE TABLE Post (
	                Id INT PRIMARY KEY IDENTITY(1, 1),
	                CoverImage VARCHAR(100),
	                Title VARCHAR(100) NOT NULL,
	                Html VARCHAR(MAX) NOT NULL,
	                DateCreated DATETIME NOT NULL
                )";
            db.Execute(sql);

            var rowsAffected = db.Insert(new Post {
                Title = "abc", Html = "def"
            });
            var post = db.Select<Post>(1);

            Assert.IsTrue(rowsAffected == 1);
            Assert.IsTrue(post.Title == "abc");
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
