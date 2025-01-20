namespace Tent.Pages;
using Tent.Logic;
public class IndexModel:AuthenticatedPage
{
  public IndexModel(ICrypto cryptographer)
  :base(cryptographer) {}

  public void OnGet() {
    var userCookie = GetUserCookie();
    ViewData["LoggedIn"] = userCookie.Authenticated.ToString();
    ViewData["Email"] = userCookie.Email;
  }
}