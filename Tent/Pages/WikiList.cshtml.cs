namespace Tent.Pages;
public class WikiListModel:BasePg
{
  public List<Wiki.Wiki> WikiList = new();
  public void OnGet() =>
    WikiList = db.Sel<Wiki.Wiki>();
}