using System;

namespace Tent.Pages.blog
{
    public class PostPage : PostBasePage
    {
    }
    public class PostBasePage : BasePage
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

        public void OnPost() {
            var save = Request.Form["save"].ToStringOr(null);
            if (save == "Save") {
                var html = Request.Form["html"].ToString();
                var slug = RouteData.Values["slug"].ToString();
                var post = db.SelectOne<Post>("WHERE Slug = @Slug", slug);
                post.Body = html;
                db.Update(post);
                Post = post;
            }
            
            var cancel = Request.Form["cancel"].ToStringOr(null);
            if (cancel == "Cancel") {
                Response.Redirect(Request.Path);
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