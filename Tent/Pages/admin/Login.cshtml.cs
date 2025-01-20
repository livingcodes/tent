namespace Tent.Pages.Admin;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

public class Login
{
  [Required, EmailAddress]
  public str Email { get; set; }

  [Required]
  [StringLength(32, MinimumLength = 8)]
  [DataType(DataType.Password)]
  public str Password { get; set; }
}

public class LoginPage : AthPg
{
  public LoginPage(
    ILogger<LoginPage> logger,
    Logic.ICrypto cryptographer
  ) : base(cryptographer) {
    this.logger = logger;
  }

  ILogger<LoginPage> logger;

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
         
    var result = new Auth.Login(Login.Email, Login.Password).Exe();
    if (result.Failed) {
      ModelState.AddModelError("Login", result.ErrMsg);
      return Page();
    }
    
    var usr = result.Val;
    SetUsrCkie(usr);
    return RedirectToPage("/Index");
  }
}