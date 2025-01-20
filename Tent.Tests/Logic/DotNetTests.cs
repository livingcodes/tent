namespace Tent.Tests.Logic;
[tc]public class DotNetTests:BaseTests
{
  /*
  // Can replace
  public object Instance 
  {
    get 
    {
      if (instance == null)
      {
        instance = new Object()
      }
      return instance;
    }
  }
  object instance;

  // with functionally equivalent but more succinct (12 lines to 2)
  public object Instance => instance ?? (instance = new Object());
  object instance;
  */

  [tm]public void NullCoalescingOperator() {
    var val = new Val(2);
    assert(val.AsInt == 2);
    assert(val.AsString == "2");
    // does NOT increment b/c private asString is set (not null)
    assert(val.AsStringPlus1 == "2");

    val = new Val(2);
    assert(val.AsInt == 2);
    // does increment b/c private asString is null
    assert(val.AsStringPlus1 == "3");
    assert(val.AsString == "3");
    // does NOT increment b/c private asString is set now (not null)
    assert(val.AsStringPlus1 == "3");
  }
}

class Val(int input = 0)
{
  public int AsInt { get; } = input;

  public str AsString => asString ?? (asString = AsInt.ToString());
  public str AsStringPlus1 => asString ?? (asString = (AsInt + 1).ToString());
  str asString;
}