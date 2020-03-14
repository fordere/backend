using System;
using System.Net;
using System.Net.Mail;
using ServiceStack.Configuration;

namespace forderebackend.ServiceInterface.Smtp
{
    /// <summary>
    /// Wrapper class arround SmtpClient to use our configuration (app.config).
    /// </summary>
    public class SmtpClientWrapper
    {
        public static void Send(string recipients, string subject, string body, string replyTo = null)
        {
            var config = new AppSettings();

            var smtpEnabled = config.Get("Smtp.Enabled", false);

            if (smtpEnabled)
            {
                var host = config.GetString("Smtp.Host");
                var port = config.Get("Smtp.Port", 25);
                var sender = config.Get("Smtp.Sender", "info@fordere.ch");

                var mail = new MailMessage(sender, recipients, subject, body);
                mail.ReplyToList.Add(new MailAddress(replyTo ?? sender));

                var client = new SmtpClient(host, port);
                client.EnableSsl = config.Get("Smtp.EnableSsl", false);

                var useDefaultCredentials = config.Get("Smtp.UseDefaultCredentials", true);
                client.UseDefaultCredentials = useDefaultCredentials;

                if (!useDefaultCredentials)
                {
                    var userName = config.GetString("Smtp.AuthUser");
                    var password = config.GetString("Smtp.AuthPassword");
                    client.Credentials = new NetworkCredential(userName, password);

                    ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                }

                try
                {
                    client.Send(mail);
                }
                catch (Exception e)
                {
                    // TODO Handle!
                    Console.WriteLine(e);
                    Console.WriteLine("Fail on Mail");
                }
            }
        }
    }
}
