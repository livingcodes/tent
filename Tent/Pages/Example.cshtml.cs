namespace Tent.Pages;
public class ContactModel:BasePg
{
  public str Msg { get; set; }

  public void OnGet() {
    Title = "Example";
    Msg = "Model property from code-behind (PageModel)";
  }
}