namespace Tent.Common;
using Tent.Data;
public class Command {
  protected Pack db => _db ??= new Pack();
  Pack _db;
}