namespace Tent.Pages;
public class ContactModel:BasePage
{
  public string Message { get; set; }

  public void OnGet() {
    Title = "Example";
    Message = "Model property from code-behind (PageModel)";
  }
}