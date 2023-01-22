using System.Net.Mail;

namespace Tent.Logic;
public interface ISend { 
   void Send(string message);
}

public class DebugSend : ISend {
   public void Send(string message) {
      System.Diagnostics.Debug.WriteLine(message);
   }
}

// Port = 587
public class Emailer : ISend {
   public Emailer(EmailConfig config, EmailTemplate template) { }
   EmailConfig config; EmailTemplate mail;

   public void Send(string message) {
      var smtp = new SmtpClient(config.host, config.port) 
         { EnableSsl = true };
      var msg = new MailMessage
         (mail.from, mail.to, mail.subject, mail.body)
         { IsBodyHtml = true };
      smtp.Send(msg);
   }
}

public record EmailConfig(string from, string host, int port);

public record EmailTemplate(int id, string from, string to, 
   string subject, string body);