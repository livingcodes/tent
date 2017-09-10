using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace Tent.Pages.admin
{
    public class Login
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
    public class LoginModel : PageModel
    {
        public void OnGet() {
            ViewData["Title"] = "Login";
        }

        [BindProperty]
        public Login Login { get; set; }

        public async Task<IActionResult> OnPostAsync() {
            if (!ModelState.IsValid)
                return Page();

            // todo: validate login
            
            return RedirectToPage("/Index");
        }
    }
}