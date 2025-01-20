namespace Tent.Logic;
using System.Net.Mail;
public interface ISend { 
  void Send(str message);
}

public class DebugSend : ISend {
  public void Send(str message) {
    System.Diagnostics.Debug.WriteLine(message);
  }
}

// Port = 587
public class Emailer : ISend {
  public Emailer(EmlCfg config, EmlTemplate template) { }
  EmlCfg config; EmlTemplate mail;

  public void Send(string message) {
    var smtp = new SmtpClient(config.host, config.port) 
      { EnableSsl = true };
    var msg = new MailMessage
      (mail.from, mail.to, mail.subject, mail.body)
      { IsBodyHtml = true };
    smtp.Send(msg);
  }
}

public record EmlCfg(str from, str host, int port);

public record EmlTemplate(int id, str from, str to,
  str subject, str body);