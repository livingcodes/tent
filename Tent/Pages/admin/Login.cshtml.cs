using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Tent.Pages.admin
{
    public class Login
    {
        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(32, MinimumLength = 8)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
    public class LoginModel : PageModel
    {
        public LoginModel(ILogger<LoginModel> logger) {
            this.logger = logger;
        }

        ILogger<LoginModel> logger;

        public void OnGet() {
            ViewData["Title"] = "Login";
        }

        [BindProperty]
        public Login Login { get; set; }

        public async Task<IActionResult> OnPostAsync() {
            logger.LogDebug("Login post");
            // todo: required and length client-side validation doesn't prevent this http post
            if (!ModelState.IsValid)
                return Page();
            
            var login = new Logic.Login(Login.Email, Login.Password);
            var result = login.Execute();
            if (result.Failed) {
                ModelState.AddModelError("Login", result.ErrorMessage);
                return Page();
            }
            
            var user = result.Value;
            setCookie("user", user.Id.ToString());

            return RedirectToPage("/Index");
        }

        void setCookie(string key, string value) {
            Response.Cookies.Append(
                key: "user",
                value: value,
                options: new Microsoft.AspNetCore.Http.CookieOptions() {
                    Expires = DateTime.Now.AddYears(1)
                }
            );
        }
    }
}