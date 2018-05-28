using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Tent.Data;
using static Tent.Table;

namespace Tent.Tests
{
    [TestClass]
    public class SprocTests : BaseTests
    {
        public SprocTests() : base() {
            var tableName = "Post";
            db.Execute(
                new Table(tableName)
                    .AddColumn("Id", SqlType.Int, Syntax.Identity(1, 1))
                    .AddColumn("Title", SqlType.VarChar(100), Syntax.NotNull)
                    .AddColumn("Html", SqlType.VarCharMax, Syntax.NotNull)
                    .AddColumn("DateCreated", SqlType.DateTime, Syntax.NotNull)
                    .End().Sql
            );

            var admin = new Admin(db);
            admin.CreateProcedure("GetPosts", $"SELECT * FROM {tableName}");
            admin.CreateProcedure("GetPostsByDateCreated", 
                new [] { "@DateCreated DateTime" }, 
                $"SELECT * FROM {tableName} WHERE DateCreated > @DateCreated");
            admin.CreateProcedure("GetPostById", 
                new [] { "@Id INT" }, 
                $"SELECT * FROM {tableName} WHERE Id = @Id");
            
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

        [TestMethod]
        public void CacheSproc() {
            var postList = db.Sproc("GetPostsByDateCreated")
                .Cache("cached")
                .Parameter("@DateCreated", DateTime.Now.AddDays(-10))
                .Select<Post>();
            Assert.IsTrue(postList.Count > 0);
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
