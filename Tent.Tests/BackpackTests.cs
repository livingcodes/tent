using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Tent.Data;
using static Tent.Table;

namespace Tent.Tests
{
    [TestClass]
    public class BackpackTests : BaseTests
    {
        public BackpackTests() : base() {
            pack = new Pack();

            var sql = new Table("Posts")
                .AddColumn("Id", SqlType.Int, Syntax.Identity(1, 1))
                .AddColumn("Html", SqlType.VarCharMax)
                .End()
                .Sql;
            pack.Select<int>(sql);

            db.Insert(new Post() {
                Html = "abc"
            });
        }
        Pack pack;

        [TestMethod]
        public void Select() {
            var posts = pack.Select<Post>("select * from posts");
            Assert.IsTrue(posts.Count > 0);
        }

        [TestMethod]
        public void SelectWithParameterArguement() {
            var posts = pack.Select<Post>("select * from posts where id = @id", 1);
            Assert.IsTrue(posts.Count > 0);
        }

        [TestMethod]
        public void SelectWith2ParameterArguements() {
            var posts = pack.Select<Post>("select * from posts where id = @id and html = @html", 1, "abc");
            Assert.IsTrue(posts.Count > 0);
        }

        [TestMethod]
        public void SelectWithParameterFunction() {
            var posts = pack.Sql("select * from posts where id = @id")
                .Parameter("@id", 1)
                .Select<Post>();
            Assert.IsTrue(posts.Count == 1);
        }

        [TestMethod]
        public void SelectWith2ParameterFunctions() {
            var posts = pack.Sql("select * from posts where id = @id and html = @html")
                .Parameter("@id", 1)
                .Parameter("@html", "abc")
                .Select<Post>();
            Assert.IsTrue(posts.Count == 1);
        }

        [TestMethod]
        public void SelectOne() {
            var post = pack.SelectOne<Post>("select * from posts");
            Assert.IsTrue(post.Id == 1);
        }

        //[TestMethod]
        //public void Execute() {
        //    var affectedRows = pack.Execute("insert into posts values ('another')");
        //    Assert.IsTrue(affectedRows == 1);
        //}

        [TestMethod]
        public void SelectEmptyListResult() {
            var posts = pack.Select<Post>("select * from posts where id = 2");
            Assert.IsTrue(posts.Count == 0);
        }

        [TestMethod]
        public void SelectWithParameterUsedMultipleTimes() {
            var posts = pack.Select<Post>("select * from posts where id = @id and id < (@id + 1) and html = @html", 1, "abc");
            Assert.IsTrue(posts.Count == 1);
        }

        [TestMethod, ExpectedException(typeof(Exception))]
        public void ParameterCountDoesNotMatch() {
            var posts = pack.Select<Post>("select * from posts where id = @id and html = @html", 1);
            // throws exception
        }

        public class Post
        {
            public int Id { get; set; }
            public string Html { get; set; }
        }
    }
}
