using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Tent.Pages
{
    public class AboutModel : PageModel
    {
        public string Message { get; set; }

        public void OnGet() {
            Message = "From OnGet() code-behind.";
        }
    }
}
