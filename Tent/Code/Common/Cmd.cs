namespace Tent.Common;
using Tent.Data;
public class Cmd {
  protected Pack db => _db ??= new Pack();
  Pack _db;
}