using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tent.Data;

namespace Tent.Tests
{
    [TestClass]
    public class DatabaseStructTests : BaseTests
    {
        [TestMethod]
        public void SelectToStructList() {
            var pack = new Pack();
            var posts = pack.Select<Post>("select * from posts");
            Assert.IsTrue(posts[0].Id == 1);
        }

        [TestMethod]
        public void SelectToStruct() {
            var pack = new Pack();
            var post = pack.SelectOne<Post>("select top 1 * from posts");
            Assert.IsTrue(post.Id == 1);
        }

        public struct Post
        {
            public int Id { get; set; }
            public string Html { get; set; }
        }
    }
}
