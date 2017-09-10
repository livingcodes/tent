using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Tent.Pages.admin
{
    public class Login
    {
        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, StringLength(32, MinimumLength = 8)]
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
            // todo: required and length client-side validation doesn't prevent this http post
            if (!ModelState.IsValid)
                return Page();
            
            // todo: login using database
            if (Login.Email == "admin@tent.com"
            && Login.Password == "password")
                return RedirectToPage("/Index");
            
            return Page();
        }
    }
}