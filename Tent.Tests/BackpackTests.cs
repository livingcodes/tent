using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tent.Data;

namespace Tent.Tests
{
    [TestClass]
    public class BackpackTests : BaseTests
    {
        public BackpackTests() : base() {
            pack = new Pack();
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
        public void SelectWithParameter() {
            var posts = pack.Select<Post>("select * from posts where id = @id", 2);
            Assert.IsTrue(posts.Count > 0);
        }

        [TestMethod]
        public void SelectWith2Parameters() {
            var posts = pack.Select<Post>("select * from posts where id = @id and html = @html", 1, "abc");
            Assert.IsTrue(posts.Count > 0);
        }

        public class Post
        {
            public int Id { get; set; }
            public string Html { get; set; }
        }
    }
}
