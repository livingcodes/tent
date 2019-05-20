namespace Tent.Wiki
{
    public class WikiPageModel : BasePage
    {
        public string Body;

        public void OnGet() {
            var slug = RouteData.Values["slug"].ToStringOr(null);
            if (slug == null) { 
                Title = "Slug not found";
                return;
            }
            var article = db.SelectOne<Wiki>("WHERE Slug = @Slug", slug);
            Title = article.Title;
            Body = article.Body;
        }
    }
}