using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using Tent.Logic;
using static Shorthand;

namespace Tent
{
    public class AuthenticatedPage : PageModel
    {
        public AuthenticatedPage(ICryptographer cryptographer) =>
            this.cryptographer = cryptographer;

        public UserCookie GetUserCookie() =>
            Try(
                () => userCookie._(decrypt)._(deserialize),
                () => new UserCookie()
            );
        
        public void SetUserCookie(UserCookie cookie) =>
            setCookie( "user", cookie._(toJson)._(encrypt), Now.AddYears(1) );

        public void SetUserCookie(User user) =>
            user._(buildUserCookie)._(SetUserCookie);

        #region private
        ICryptographer cryptographer;
        string userCookie => Request.Cookies["user"];
        string toJson(object x) =>
            Newtonsoft.Json.JsonConvert.SerializeObject(x);
        string encrypt(string json) =>
            cryptographer.Encrypt(json);
        UserCookie deserialize(string json) =>
            Newtonsoft.Json.JsonConvert.DeserializeObject<UserCookie>(json);
        string decrypt(string cookie) =>
            cryptographer.Decrypt(cookie); // exception if null (i.e. cookie missing)
        void setCookie(string key, string value, DateTime expiration) =>
            Response.Cookies.Append(
                key: key,
                value: value,
                options: new Microsoft.AspNetCore.Http.CookieOptions() {
                    Expires = expiration
                }
            );
        UserCookie buildUserCookie(User user) =>
            user == null
                ? new UserCookie()
                : new UserCookie() {
                    Authenticated = true,
                    Id = user.Id,
                    Email = user.Email
                };
        #endregion
    }

    public class UserCookie {
        public int Id { get; set; }
        public string Email { get; set; }
        public bool Authenticated { get; set; }
    }
}