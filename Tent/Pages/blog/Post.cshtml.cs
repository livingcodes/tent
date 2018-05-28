using System;
using Tent.Data;

namespace Tent.Pages.blog
{
    public class PostPage : BasePage
    {
        public void OnGet() {
            var db = new Pack();

            if (!Request.QueryString.HasValue) {
                var mostRecent = db.SelectOne<Post>(
                    @"SELECT TOP 1 * FROM Posts 
                    ORDER BY PublishDate DESC");
                Post = mostRecent;
            } else {
                var queryString = Request.QueryString.Value;
                var id = queryString.Split('=')[1];
                Post = db.Select<Post>(id.ToInt());
            }
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