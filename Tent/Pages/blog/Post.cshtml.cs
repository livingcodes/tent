using System;
using Tent.Data;

namespace Tent.Pages.blog
{
    public class PostPage : BasePage
    {
        public void OnGet() {
            if (RouteData.Values.ContainsKey("slug")) {
                var slug = RouteData.Values["slug"].ToString();
                var post = db.SelectOne<Post>("WHERE Slug = @Slug", slug);
                Post = post;
            } else if (!Request.QueryString.HasValue) {
                var mostRecent = db.SelectOne<Post>(
                    "ORDER BY PublishDate DESC");
                Post = mostRecent;
            } else {
                var queryString = Request.QueryString.Value;
                var id = queryString.Split('=')[1];
                Post = db.SelectById<Post>(id.ToInt());
            }
        }

        public Post Post { get; set; }
    }

    public class Post
    {
        public int Id { get; set; }
        public string Slug { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public DateTime PublishDate { get; set; }
    }
}