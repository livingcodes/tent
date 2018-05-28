using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tent.Data;

namespace Tent.Tests
{
    [TestClass]
    public class QueryTests : BaseTests
    {
        public QueryTests() {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile(@"c:\code\secrets\Tent\settings.json")
                .Build();
            connectionString = configuration["connectionString"];
        }

        string connectionString;

        [TestMethod]
        public void SelectSql() {
            var db = new Pack();
            db.Select<int>(@"
                IF EXISTS (
                    SELECT * FROM INFORMATION_SCHEMA.TABLES
                    WHERE TABLE_NAME = 'Post'
                )
                    DROP TABLE Post
                CREATE TABLE Post (
	                Id INT PRIMARY KEY IDENTITY(1, 1),
	                Html VARCHAR(MAX) NOT NULL,
                )");
            db.Insert(new Post() { Html = "abc" });

            var connectionFactory = new SqlConnectionFactory();
            var reader = new Reader();
            var query = new Query(connectionFactory, reader);
            var posts = query.Select<Post>("SELECT * FROM Post"); 

            Assert.IsTrue(posts.Count == 1);
        }

        class Post {
            public int Id { get; set; }
            public string Html { get; set; }
        }
    }
}
