namespace Tent;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tent.Common;

public class BasePage : PageModel
{
   /// <summary>Title shown in browser</summary>
   public string Title {
      get { return ViewData["Title"].ToStringOr(""); }
      set { ViewData["Title"] = value; }
   }

   public Data.Pack db { get {
      if (_db == null)
         _db = new Data.Pack();
      return _db;
   } }
   Data.Pack _db;

   protected string Form(string name) =>
      Request.Form[name].FirstOrDefault();

   protected string QueryString(string key) =>
      Request.Query[key].FirstOrDefault();
}