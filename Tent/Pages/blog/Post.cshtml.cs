using System;
using Tent.Data;

namespace Tent.Pages.blog
{
    public class PostPage : BasePage
    {
        public void OnGet() {
            var db = new Pack();
            var posts = db.Select<Post>(
                @"SELECT TOP 1 * FROM Posts 
                ORDER BY PublishDate DESC");
            Post = posts[0];
        }

        public Post Post { get; set; }
    }

    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public DateTime PublishDate { get; set; }
    }
}