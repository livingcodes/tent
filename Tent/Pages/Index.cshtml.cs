using System;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tent.Logic;

namespace Tent.Pages
{
    public class IndexModel : AuthenticatedPageModel
    {
        public void OnGet() {
            var userCookie = GetUserCookie();
            ViewData["LoggedIn"] = userCookie.Authenticated.ToString();
            ViewData["Email"] = userCookie.Email;
        }
    }

    public class AuthenticatedPageModel : PageModel
    {
        public UserCookie GetUserCookie() {
            var cookie = Request.Cookies["user"];
            if (cookie == null)
                return new UserCookie();
            
            UserCookie deserialized = null;
            try {
                deserialized = Newtonsoft.Json.JsonConvert.
                    DeserializeObject<UserCookie>(cookie);
            } catch (Exception ex) {
                return new UserCookie();
            }
            return deserialized;
        }

        public void SetUserCookie(UserCookie userCookie) {
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(userCookie);
            SetCookie("user", json, DateTime.Now.AddYears(1));
        }

        public void SetUserCookie(User user) {
            var userCookie = user == null
                ? new UserCookie()
                : new UserCookie() {
                    Authenticated = true,
                    Id = user.Id,
                    Email = user.Email
                };
            SetUserCookie(userCookie);
        }

        void SetCookie(string key, string value, DateTime expiration) {
            Response.Cookies.Append(
                key: key,
                value: value,
                options: new Microsoft.AspNetCore.Http.CookieOptions() {
                    Expires = expiration
                }
            );
        }
    }

    public class UserCookie
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public bool Authenticated { get; set; }
    }
}
