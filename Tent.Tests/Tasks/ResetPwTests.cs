namespace Tent.Tests.Tasks;
using Tent.Auth.Password;
using Tent.Logic;
[tc, Ignore]
public class ResetPwTests:BaseTests {
  [tm]public void ResetFailed() {
    var result = new ResetPw(1000000).Exe();
    assert(result.Failed);
  }

  [tm]public void ResetSuccess() {
    var x = new ResetPw(1);
    var result = x.Exe();
    Assert.IsTrue(result.Val.IsSet());
  }

  [tm]public void SendFailed() {
    ISend send = new DebugSend();
    var x = new SendCode("oops", send);
    var result = x.Exe();
    assert(result.Failed);
  }

  [tm]public void SendSuccess() {
    ISend send = new DebugSend();
    var x = new SendCode("case@sparkle.stream", send);
    var result = x.Exe();
    assert(!result.Failed);
  }

  [tm]public void VerifySuccess() {
    var x = new VerifyCode(1, "27450d68-9255-4cc9-9167-5b4211401d46");
    var result = x.Exe();
    assert(result.Val.IsReset);
  }
}