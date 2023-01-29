namespace Tent;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Reflection;
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
   
   protected T Form<T>() {
      var inst = Activator.CreateInstance<T>();
      var properties = typeof(T).GetProperties(BindingFlags.Public| BindingFlags.Instance);
      foreach (var property in properties) {
         if (Request.Form.ContainsKey(property.Name))
            property.SetValue(inst, Form(property.Name));
      }
      var fields = typeof(T).GetFields(BindingFlags.Public | BindingFlags.Instance);
      foreach (var field in fields) {
         if (Request.Form.ContainsKey(field.Name)) {
            string formString = Form(field.Name);
            object value = Convert.ChangeType(formString, field.FieldType);
            field.SetValue(inst, value);
         }
      }
      return inst;
   }

   protected string QueryString(string key) =>
      Request.Query[key].FirstOrDefault();
}