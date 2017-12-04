using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Tent.Data;

namespace Tent.Tests
{
    [TestClass]
    public class DatabaseStructTests : BaseTests
    {
        public DatabaseStructTests() : base() {
            var sql = @"
                IF EXISTS (
                    SELECT * FROM INFORMATION_SCHEMA.TABLES
                    WHERE TABLE_NAME = 'Posts'
                )
                    DROP TABLE Posts
                CREATE TABLE Posts (
	                Id INT PRIMARY KEY IDENTITY(1, 1),
	                Html VARCHAR(MAX) NOT NULL,
	                PublishDate DATETIME NOT NULL
                )";
            db.Execute(sql);

            actual = new Post() {
                Html = "abc",
                PublishDate = new DateTime(2018, 1, 1)
            };
            db.Insert(actual);
        }
        Post actual;

        [TestMethod]
        public void SelectToStructList() {
            var posts = db.Select<Post>("select * from posts");
            Assert.IsTrue(posts[0].Id == 1);
        }

        [TestMethod]
        public void SelectToStruct() {
            var post = db.SelectOne<Post>("select top 1 * from posts");
            Assert.IsTrue(post.Id == 1);
            Assert.IsTrue(post.Html == "abc");
        }

        [TestMethod]
        public void SelectStringList() {
            var htmlList = db.Select<string>("select html from posts");
            Assert.IsTrue(htmlList[0] == "abc");
        }

        [TestMethod]
        public void SelectString() {
            var html = db.SelectOne<string>("select top 1 html from posts");
            Assert.IsTrue(html == "abc");
        }

        [TestMethod]
        public void SelectIntList() {
            var id = db.Select<int>("select id from posts");
            Assert.IsTrue(id[0] == 1);
        }

        [TestMethod]
        public void SelectInt() {
            var id = db.SelectOne<int>("select top 1 id from posts");
            Assert.IsTrue(id == 1);
        }

        [TestMethod]
        public void SelectDateTimeList() {
            var dates = db.Select<DateTime>("select publishdate from posts");
            Assert.IsTrue(dates[0] == actual.PublishDate);
        }

        [TestMethod]
        public void SelectDateTime() {
            var date = db.SelectOne<DateTime>("select top 1 publishdate from posts");
            Assert.IsTrue(date == actual.PublishDate);
        }

        public struct Post
        {
            public int Id { get; set; }
            public string Html { get; set; }
            public DateTime PublishDate { get; set; }
        }
    }
}
