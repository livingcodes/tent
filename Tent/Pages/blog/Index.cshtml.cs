namespace Tent.Pages.Blog;
using System.Collections.Generic;
public class IndexModel : BasePage
{
   public void OnGet() {
      Posts = db
         .Paging(number:1, size:2)
         .Select<Post>("ORDER BY PublishDate DESC");
   }

   public List<Post> Posts { get; set; }
}