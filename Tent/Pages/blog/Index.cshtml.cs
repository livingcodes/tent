namespace Tent.Pages.Blog;
using System.Collections.Generic;
public class IndexModel : BasePage
{
   public void OnGet() {
      Posts = db
         .Pg(num:1, sz:2)
         .Sel<Post>("ORDER BY PublishDate DESC");
   }

   public List<Post> Posts { get; set; }
}