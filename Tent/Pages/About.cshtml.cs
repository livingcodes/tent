namespace Tent.Pages;
using Microsoft.AspNetCore.Mvc.RazorPages;
public class AboutModel : PageModel
{
  public str Msg { get; set; }

  public void OnGet() {
    Msg = "From OnGet() code-behind.";
  }
}