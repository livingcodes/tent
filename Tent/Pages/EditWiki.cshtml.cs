namespace Tent.Wiki;
public class EditWikiModel:BasePg
{
  public Wiki Entry;

  public void OnGet() {
    var slug = Rte("slug");
    if (slug == null) {
      Title = "Slug not found.";
      return;
    }

    Entry = db.Sel1<Wiki>("WHERE Slug = @Slug", slug);
  }

  public void OnPost() {
    if (Frm("cancel") == "Cancel") {
      Response.Redirect(Request.Path);
      return;
    }

    str save = Frm("save");
    if (save == "Save") {
      str slug = Rte("slug");
      var entry = db.Sel1<Wiki>("WHERE Slug = @Slug", slug);
      entry.Title = Frm("title");
      entry.Body = Frm("body");
      db.Upd(entry);
      Entry = entry;
    }
  }
}