﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Ase.Table;

namespace Ase.Tests
{
    [TestClass]
    public class DbTests : BaseTests
    {
        [ClassInitialize]
        public static void InitializeClass(TestContext context) {
            initialize();
            var sql = new Table("Post")
                .AddColumn("Id", SqlType.Int, Syntax.Identity(1, 1))
                .AddColumn("Html", SqlType.VarChar(200))
                .End()
                .Sql;
            db.Execute(sql);
        }

        [TestMethod]
        public void InsertReturns() {
            db.Admin.Truncate("Post");
            var (id, rowsAffected) = db.Insert(new Post { Html = "A" });
            assert(id == 1);
            assert(rowsAffected == 1);

            (id, rowsAffected) = db.Insert(new Post { Html = "B" });
            assert(id == 2);
            assert(rowsAffected == 1);
        }

        [TestMethod]
        public void InsertSelectUpdateDelete() {
            db.Admin.Truncate("Post");
            
            var (id, rows) = db.Insert(new Post { Html="B" });
            assert(id == 1);
            assert(rows == 1);

            var posts = db.Select<Post>("select * from post where id = 1");
            assert(posts.Count == 1);
            var post = posts[0];
            assert(post.Html == "B");
            
            post.Html = "c";
            rows = db.Update(post);
            assert(rows == 1);
            posts = db.Select<Post>("select * from post where id = 1");
            post = posts[0];
            assert(post.Html == "c");

            rows = db.Delete<Post>(1);
            assert(rows == 1);
            posts = db.Select<Post>("select * from post where id = 1");
            assert(posts.Count == 0);
        }

        public class Post { 
            public int Id { get; set; }
            public string Html { get; set; } 
        }
    }
}
