using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tent.Tests.Logic
{
    [TestClass]
    public class DotNetTests
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

        [TestMethod]
        public void NullCoalescingOperator() {
            var value = new Value(2);
            Assert.IsTrue(value.AsInt == 2);
            Assert.IsTrue(value.AsString == "2");
            // does NOT increment b/c private asString is set (not null)
            Assert.IsTrue(value.AsStringPlus1 == "2");

            value = new Value(2);
            Assert.IsTrue(value.AsInt == 2);
            // does increment b/c private asString is null
            Assert.IsTrue(value.AsStringPlus1 == "3");
            Assert.IsTrue(value.AsString == "3");
            // does NOT increment b/c private asString is set now (not null)
            Assert.IsTrue(value.AsStringPlus1 == "3");
        }
    }

    class Value
    {
        public Value(int input = 0) {
            AsInt = input;
        }

        public string AsString => asString ?? (asString = AsInt.ToString());
        public string AsStringPlus1 => asString ?? (asString = (AsInt + 1).ToString());
        string asString;

        public int AsInt { get; }
    }
}
