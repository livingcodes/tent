using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Tent.Tests
{
    [TestClass]
    public class DatabaseNullableTests : BaseTests
    {
        public DatabaseNullableTests() {
            var sql = @"
                IF EXISTS (
                    SELECT * FROM INFORMATION_SCHEMA.TABLES
                    WHERE TABLE_NAME = 'Post'
                )
                    DROP TABLE Post
                CREATE TABLE Post (
	                Id INT PRIMARY KEY IDENTITY(1, 1),
	                Html VARCHAR(MAX) NOT NULL,
                    OptIn BIT,
                )";
            db.Execute(sql);
        }
        Post actual;

        [TestMethod]
        public void GetNullableTrue() {
            actual = new Post() {
                Html = "abc",
                OptIn = true
            };
            db.Insert(actual);

            var post = db.SelectById<Post>(1);
            assert(post.OptIn.Value);
        }

        [TestMethod]
        public void GetNullableFalse() {
            actual = new Post() {
                Html = "abc",
                OptIn = false
            };
            db.Insert(actual);

            var post = db.SelectById<Post>(1);
            assert(post.OptIn.Value == false);
        }

        [TestMethod]
        public void GetNullableBoolNull() {
            db.Execute("insert into post (html) values ('abc')");

            var post = db.SelectById<Post>(1);
            assert(post.OptIn.HasValue == false);
        }

        public class Post
        {
            public int Id { get; set; }
            public string Html { get; set; }
            public Nullable<bool> OptIn { get; set; }
        }
    }
}
