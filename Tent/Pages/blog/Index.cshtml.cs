using System.Collections.Generic;
using Tent.Data;

namespace Tent.Pages.blog
{
    public class IndexModel : BasePage
    {
        public void OnGet() {
            var db = new Pack();
            Posts = db.Select<Post>("SELECT * FROM Post ORDER BY PublishDate DESC");
        }

        public List<Post> Posts { get; set; }
    }
}