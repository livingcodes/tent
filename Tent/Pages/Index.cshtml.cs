using Tent.Logic;

namespace Tent.Pages
{
    public class IndexModel : AuthenticatedPage
    {
        public IndexModel(ICryptographer cryptographer) : base(cryptographer) {}

        public void OnGet() {
            var userCookie = GetUserCookie();
            ViewData["LoggedIn"] = userCookie.Authenticated.ToString();
            ViewData["Email"] = userCookie.Email;
        }
    }
}
