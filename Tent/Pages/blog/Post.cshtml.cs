namespace Tent.Pages.Blog;
using System;
using Tent.Common;

public class PostPage : PostBasePage {}

public class PostBasePage : BasePage {
   public void OnGet() {
      if (RouteData.Values.ContainsKey("id")) {
         var id = RouteData.Values["id"].ToStringOr("").ToInt();
         Post = db.SelectById<Post>(id);
      } else if (RouteData.Values.ContainsKey("slug")) {
         var slug = RouteData.Values["slug"].ToString();
         Post = db.SelectOne<Post>("WHERE Slug = @Slug", slug);
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
         var id = Request.Form["Id"].ToString().ToInt();
         var title = Request.Form["title"].ToString();
         var html = Request.Form["html"].ToString();
         var slug = Request.Form["slug"].ToString();
         if (id > 0) {
            var post = db.SelectById<Post>(id);
            post.Slug = slug;
            post.Title = title;
            post.Body = html;
            post.PublishDate = Now;
            db.Update(post);
            Post = post;
         } else {
            var post = new Post();
            post.Slug = slug;
            post.Title = title;
            post.Body = html;
            post.PublishDate = Now;
            (post.Id, _) = db.Insert(post);
            Post = post;
         }
      }
            
      var cancel = Request.Form["cancel"].ToStringOr(null);
      if (cancel == "Cancel") {
         Response.Redirect(Request.Path);
      }
   }

   public Post Post { get; set; }
}

public class Post {
    public int Id;
    public string Slug, Title, Body;
    public DateTime PublishDate;
}