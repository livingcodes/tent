namespace Tent.Pages.Blog;
using System;
using Tent.Common;

public class PostPage : PostBasePage {}

public class PostBasePage : BasePage {
  public void OnGet() {
    if (RouteData.Values.ContainsKey("id")) {
      var id = RouteData.Values["id"].ToStringOr("").ToInt();
      Post = db.SelById<Post>(id);
    } else if (RouteData.Values.ContainsKey("slug")) {
      var slug = RouteData.Values["slug"].ToString();
      Post = db.Sel1<Post>("WHERE Slug = @Slug", slug);
    } else if (!Request.QueryString.HasValue) {
      var mostRecent = db.Sel1<Post>(
        "ORDER BY PublishDate DESC");
      Post = mostRecent;
    } else {
      var queryString = Request.QueryString.Value;
      var id = queryString.Split('=')[1];
      Post = db.SelById<Post>(id.ToInt());
    }
  }

  public void OnPost() {
    if (Form("save") == "Save") {
      Post = Form<Post>();
      if (Post.Id > 0) {
        var post = db.SelById<Post>(Post.Id);
        if (post == null)
          return; // post id not found
        Post.PublishDate = Now;
        db.Upd(Post);
      } else {
        Post.PublishDate = Now;
        (Post.Id, _) = db.Ins(Post);
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