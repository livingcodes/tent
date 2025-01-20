namespace Tent.Tests.Tasks;
using Tent.Auth.Password;
using Tent.Logic;
[TestClass, Ignore]
public class ResetPasswordTests:BaseTests {
  [TestMethod] public void ResetFailed() {
    var x = new ResetPassword(1000000);
    var result = x.Execute();
    Assert.IsTrue(result.Failed);
  }

  [TestMethod] public void ResetSuccess() {
    var x = new ResetPassword(1);
    var result = x.Execute();
    Assert.IsTrue(result.Value.IsSet());
  }

  [TestMethod] public void SendFailed() {
    var send = new DebugSend();
    var x = new SendCode("oops", send);
    var result = x.Execute();
    Assert.IsTrue(result.Failed);
  }

  [TestMethod] public void SendSuccess() {
    var send = new DebugSend();
    var x = new SendCode("case@sparkle.stream", send);
    var result = x.Execute();
    assert(!result.Failed);
  }

  [TestMethod] public void VerifySuccess() {
    var x = new VerifyCode(1, "27450d68-9255-4cc9-9167-5b4211401d46");
    var result = x.Execute();
    assert(result.Value.IsReset);
  }
}