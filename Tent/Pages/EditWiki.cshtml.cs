namespace Tent.Wiki
{
    public class EditWikiModel : BasePage
    {
        public Wiki Entry;

        public void OnGet() {
            var slug = RouteData.Values["slug"].ToStringOr(null);
            if (slug == null) {
                Title = "Slug not found.";
                return;
            }

            Entry = db.SelectOne<Wiki>("WHERE Slug = @Slug", slug);
        }

        public void OnPost() {
            var cancel = Request.Form["cancel"].ToStringOr(null);
            if (cancel == "Cancel") {
                Response.Redirect(Request.Path);
                return;
            }

            var save = Request.Form["save"].ToStringOr(null);
            if (save == "Save") {
                var slug = RouteData.Values["slug"].ToString();
                var entry = db.SelectOne<Wiki>("WHERE Slug = @Slug", slug);
                entry.Title = Request.Form["title"].ToStringOr(null);
                entry.Body = Request.Form["body"].ToStringOr(null);
                db.Update(entry);
                Entry = entry;
            }
        }
    }
}