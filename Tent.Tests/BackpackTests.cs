﻿using Ase;
using static Ase.Table;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Tent.Data;

namespace Tent.Tests
{
    [TestClass]
    public class BackpackTests : BaseTests
    {
        public BackpackTests() : base() {
            // i wasn't able to figure out how to construct distributed memory cache
            // the IOptions in particular
            // so i used the service provider to build it
            IServiceCollection services = new ServiceCollection();
            services.AddDistributedMemoryCache();
            var serviceProvider = services.BuildServiceProvider();
            var distributedCache = serviceProvider.GetService<IDistributedCache>();

            var cache = new SerializedCached(distributedCache);
            pack = new Pack(cache);

            var sql = new Table("Post")
                .AddColumn("Id", SqlType.Int, Syntax.Identity(1, 1))
                .AddColumn("Html", SqlType.VarCharMax)
                .End()
                .Sql;
            pack.Execute(sql);

            db.Insert(new Post() {
                Html = "abc"
            });
        }
        Pack pack;

        [TestMethod]
        public void Select() {
            var posts = pack.Select<Post>("select * from post");
            Assert.IsTrue(posts.Count > 0);
        }

        [TestMethod]
        public void CacheSelectList() {
            var posts = pack.Cache("test").Select<Post>("select * from post");
            Assert.IsTrue(posts.Count == 1);

            db.Insert(new Post() { Html = "Test 2" });
            posts = pack.Select<Post>("select * from post");
            Assert.IsTrue(posts.Count == 2);

            posts = pack.Cache("test").Select<Post>("select * from post");
            Assert.IsTrue(posts.Count == 1);
        }

        [TestMethod]
        public void CacheSelectOne() {
            // original html is abc
            var post = pack.Cache("test-one").SelectOne<Post>("select * from post where id = 1");
            Assert.IsTrue(post.Html == "abc");

            // update html to def
            post.Html = "def";
            db.Update(post);

            // get uncached, updated html from database
            var postUpdated = pack.SelectOne<Post>("select * from post where id = 1");
            Assert.IsTrue(postUpdated.Html == "def");

            // get cached, original html from cache
            var cachedPost = pack.Cache("test-one").SelectOne<Post>("select * from post where id = 1");
            Assert.IsTrue(cachedPost.Html == "abc");
        }

        [TestMethod]
        public void CacheSelectById() {
            var post = pack.Cache("by-id").SelectById<Post>(1);
            Assert.IsTrue(post.Html == "abc");

            post.Html = "def";
            db.Update(post);
            
            var updatedPost = pack.SelectById<Post>(1);
            Assert.IsTrue(updatedPost.Html == "def");

            var cachedPost = pack.Cache("by-id").SelectById<Post>(1);
            Assert.IsTrue(cachedPost.Html == "abc");
        }

        [TestMethod]
        public void SelectWithParameterArguement() {
            var posts = pack.Select<Post>("select * from post where id = @id", 1);
            Assert.IsTrue(posts.Count > 0);
        }

        [TestMethod]
        public void SelectWith2ParameterArguements() {
            var posts = pack.Select<Post>("select * from post where id = @id and html = @html", 1, "abc");
            Assert.IsTrue(posts.Count > 0);
        }

        [TestMethod]
        public void SelectWithParameterFunction() {
            var posts = pack.Sql("select * from post where id = @id")
                .Parameter("@id", 1)
                .Select<Post>();
            Assert.IsTrue(posts.Count == 1);
        }

        [TestMethod]
        public void SelectWith2ParameterFunctions() {
            var posts = pack.Sql("select * from post where id = @id and html = @html")
                .Parameter("@id", 1)
                .Parameter("@html", "abc")
                .Select<Post>();
            Assert.IsTrue(posts.Count == 1);
        }

        [TestMethod]
        public void SelectOne() {
            var post = pack.SelectOne<Post>("select * from post");
            Assert.IsTrue(post.Id == 1);
        }

        [TestMethod]
        public void SelectOneWithParameter() {
            var post = pack.SelectOne<Post>("select * from post where id = @id", 1);
            Assert.IsTrue(post.Id == 1);
        }

        [TestMethod]
        public void SelectById() {
            var post = pack.SelectById<Post>(1);
            Assert.IsTrue(post.Id == 1);
        }

        [TestMethod]
        public void SelectPaging() {
            for (var i=1; i<=8; i++)
                pack.Insert(new Post { Html = "Paging " + i });
            var page2 = pack
                .Sql("select * from post where html like 'Paging%' order by id")
                .Paging(2, 3)
                .Select<Post>();
            Assert.IsTrue(page2.Count == 3);
            Assert.IsTrue(page2[0].Html == "Paging 4");
        }

        [TestMethod]
        public void SelectPagingWithParameter() {
            for (var i = 1; i <= 8; i++)
                pack.Insert(new Post { Html = "Paging " + i });
            var page2 = pack
                .Sql("select * from post where html like @Keyword + '%' order by id")
                .Parameter("@Keyword", "Paging")
                .Paging(2, 3)
                .Select<Post>();
            Assert.IsTrue(page2.Count == 3);
            Assert.IsTrue(page2[0].Html == "Paging 4");
        }

        [TestMethod]
        public void Execute() {
            var affectedRows = pack.Execute("insert into post values ('another')");
            Assert.IsTrue(affectedRows == 1);

            affectedRows = pack.Execute("insert into post values ('third'); insert into post values ('fourth')");
            Assert.IsTrue(affectedRows == 2);
        }

        [TestMethod]
        public void ExecuteWithParameter() {
            var affectedRows = pack.Execute("insert into post values (@post)", "cat lols");
            Assert.IsTrue(affectedRows == 1);
        }

        [TestMethod]
        public void SelectEmptyListResult() {
            var posts = pack.Select<Post>("select * from post where id = 2");
            Assert.IsTrue(posts.Count == 0);
        }

        [TestMethod]
        public void SelectWithParameterUsedMultipleTimes() {
            var posts = pack.Select<Post>("select * from post where id = @id and id < (@id + 1) and html = @html", 1, "abc");
            Assert.IsTrue(posts.Count == 1);
        }

        [TestMethod]
        public void SelectShortSyntaxWhere() {
            var posts = pack.Select<Post>("where html = @html", "abc");
            assert(posts.Count > 0);
        }

        [TestMethod]
        public void SelectShortSyntaxOrderBy() {
            var posts = pack.Select<Post>("order by id");
            assert(posts.Count > 0);
        }

        [TestMethod]
        public void SelectOneShortSyntaxWhere() {
            var post = pack.SelectOne<Post>("where html = @html", "abc");
            assert(post.Html == "abc");
        }

        [TestMethod, ExpectedException(typeof(Exception))]
        public void ParameterCountDoesNotMatch() {
            var posts = pack.Select<Post>("select * from post where id = @id and html = @html", 1);
            // throws exception
        }

        public class Post
        {
            public int Id { get; set; }
            public string Html { get; set; }
        }
    }
}
