namespace Tent.Common;
using Tent.Data;
public class Command {
   protected Pack db {
      get {
         if (_db == null)
            _db = new Pack();
         return _db;
      }
   }
   Pack _db;
}