using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Tent.Pages.blog;
using static Tent.Table;

namespace Tent.Tests.Tasks
{
    [TestClass]
    public class BuildDatabase : BaseTests
    {
        [TestMethod]
        public void CreatePostsTable() {
            var sql = new Table("Post")
                .AddColumn("Id", SqlType.Int, Syntax.Identity(1, 1))
                .AddColumn("Slug", SqlType.VarChar(100))
                .AddColumn("Title", SqlType.VarChar(100), Syntax.NotNull)
                .AddColumn("Body", SqlType.VarCharMax, Syntax.NotNull)
                .AddColumn("PublishDate", SqlType.DateTime, Syntax.NotNull)
                .End()
                .Sql;
            var affectedRows = db.Execute(sql);
            Assert.IsTrue(affectedRows == -1);

            InsertPost();
        }

        public void InsertPost() {
            db.Insert(new Post {
                Slug = "hello",
                Title = "Hi",
                Body = "hello...",
                PublishDate = new DateTime(2018, 1, 1)
            });
            db.Insert(new Post {
                Slug = "bye",
                Title = "Goodbye",
                Body = "goodbye...",
                PublishDate = new DateTime(2018, 7, 4)
            });
            db.Insert(new Post {
                Slug = "three",
                Title = "Three",
                Body = "Three body...",
                PublishDate = new DateTime(2018, 7, 5)
            });
            db.Insert(new Post {
                Slug = "four",
                Title = "Four",
                Body = "Four body...",
                PublishDate = new DateTime(2018, 7, 6)
            });
        }
    }
}
