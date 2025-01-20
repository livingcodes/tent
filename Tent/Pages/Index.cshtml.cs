namespace Tent.Pages;
using Tent.Logic;
public class IndexModel:AthPg
{
  public IndexModel(ICrypto cryptographer)
  :base(cryptographer) {}

  public void OnGet() {
    var usrCkie = GetUsrCkie();
    ViewData["LoggedIn"] = usrCkie.Authenticated.ToString();
    ViewData["Email"] = usrCkie.Email;
  }
}