using System.Collections.Generic;
using Tent.Data;

namespace Tent.Pages.blog
{
    public class IndexModel : BasePage
    {
        public void OnGet() {
            Posts = db
                .Paging(number:1, size:2)
                .Select<Post>("ORDER BY PublishDate DESC");
        }

        public List<Post> Posts { get; set; }
    }
}