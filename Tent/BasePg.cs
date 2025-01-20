namespace Tent;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Reflection;
using Tent.Common;
public class BasePg:PageModel
{
  /// <summary>Title shown in browser</summary>
  public str Title {
    get { return ViewData["Title"].ToStringOr(""); }
    set { ViewData["Title"] = value; }
  }

  public Data.Pack db => _db ??= new Data.Pack();
  Data.Pack _db;

  protected str Frm(str name) =>
    Request.Form[name].FirstOrDefault();

  protected bln FrmHas(str key) =>
    Request.Form.ContainsKey(key);

  protected str Rte(str key) =>
    RouteData.Values[key].ToStringOr("");

  protected bln RteHas(str key) =>
    Request.RouteValues.ContainsKey(key);

  protected T Frm<T>() {
    var inst = Activator.CreateInstance<T>();
    var props = typeof(T).GetProperties(BindingFlags.Public| BindingFlags.Instance);
    foreach (var prop in props) {
      if (Request.Form.ContainsKey(prop.Name)) // FrmHas(prop.Name))
        prop.SetValue(inst, Frm(prop.Name));
    }
    var flds = typeof(T).GetFields(BindingFlags.Public | BindingFlags.Instance);
    foreach (var fld in flds) {
      if (Request.Form.ContainsKey(fld.Name)) {
        str frmStr = Frm(fld.Name);
        object val = Convert.ChangeType(frmStr, fld.FieldType);
        fld.SetValue(inst, val);
      }
    }
    return inst;
  }

  protected str QryStr(str key) =>
    Request.Query[key].FirstOrDefault();
}