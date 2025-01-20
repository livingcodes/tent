namespace Tent.Pages.Blog;
using Tent.Common;

public class PostPage : PostBasePage {}

public class PostBasePage : BasePg {
  public void OnGet() {
    if (RteHas("id")) {
      int id = Rte("id").Int();
      Post = db.SelById<Post>(id);
    } else if (RteHas("slug")) {
      str slug = Rte("slug");
      Post = db.Sel1<Post>("WHERE Slug = @Slug", slug);
    } else if (!Request.QueryString.HasValue) {
      Post = db.Sel1<Post>("ORDER BY PublishDate DESC"); // most recent
    } else {
      str qryStr = Request.QueryString.Value;
      int id = qryStr.Split('=')[1].Int();
      Post = db.SelById<Post>(id);
    }
  }

  public void OnPost() {
    if (Frm("save") == "Save") {
      Post = Frm<Post>();
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
            
    var cancel = Frm("cancel");
    if (cancel == "Cancel")
      Response.Redirect(Request.Path);
  }

  public Post Post { get; set; }
}

public class Post {
  public int Id;
  public str Slug, Title, Body;
  public dte PublishDate;
}