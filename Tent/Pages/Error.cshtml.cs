namespace Tent.Pages;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.RazorPages;
public class ErrorModel : PageModel
{
  public str RequestId { get; set; }
  public bln ShowRequestId => !string.IsNullOrEmpty(RequestId);

  public void OnGet() {
    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
  }
}