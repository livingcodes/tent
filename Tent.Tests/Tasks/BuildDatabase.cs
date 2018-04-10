using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using Tent.Pages.blog;
using static Tent.Table;

namespace Tent.Tests.Tasks
{
    [TestClass]
    public class BuildDatabase : BaseTests
    {
        [TestMethod]
        public void CreatePostsTable() {
            var sql = new Table("Posts")
                .AddColumn("Id", SqlType.Int, Syntax.Identity(1, 1))
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
                Title = "Hi",
                Body = "hello...",
                PublishDate = DateTime.Now
            });
        }
    }
}
