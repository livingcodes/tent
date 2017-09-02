using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Tent
{
    public class BasePage : PageModel
    {
        /// <summary>Title shown in browser</summary>
        public string Title {
            get { return ViewData["Title"].ToStringOr(""); }
            set { ViewData["Title"] = value; }
        }
    }
}
