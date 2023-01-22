using Tent.Common;

namespace Tent.Wiki
{
    public class WikiPageModel : BasePage
    {
        public Wiki Entry;

        public void OnGet() {
            var slug = RouteData.Values["slug"].ToStringOr(null);
            if (slug == null) { 
                Title = "Slug not found";
                return;
            }
            Entry = db.SelectOne<Wiki>("WHERE Slug = @Slug", slug);
            if (Entry == null)
                return;

            Title = Entry.Title;
        }
    }
}