namespace Tent.Wiki;
public class WikiPageModel : BasePg
{
  public Wiki Entry;

  public void OnGet() {
    var slug = Rte("slug");
    if (slug == null) { 
      Title = "Slug not found";
      return;
    }
    Entry = db.Sel1<Wiki>("WHERE Slug = @Slug", slug);
    if (Entry == null)
      return;

    Title = Entry.Title;
  }
}