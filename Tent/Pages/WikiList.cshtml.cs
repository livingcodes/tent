namespace Tent.Pages
{
    using System.Collections.Generic;

    public class WikiListModel : BasePage
    {
        public List<Wiki.Wiki> WikiList = new List<Wiki.Wiki>();
        public void OnGet() =>
            WikiList = db.Sel<Wiki.Wiki>();
    }
}