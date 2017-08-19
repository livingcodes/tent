using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Tent.Pages
{
    public class ContactModel : PageModel
    {
        public string Message { get; set; }

        public void OnGet() {
            Message = "Model property from code-behind (PageModel)";
        }
    }
}
