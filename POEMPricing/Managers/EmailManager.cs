using System;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace POEMPricing.Managers
{
    public class EmailManager
    {
        public async Task SendEmailAsync(
            string toEmail,
            string subject,
            string body)
        {
            using (var client = new SmtpClient())
            {
                client.Host = ConfigurationManager.AppSettings["SmtpServer"];
                client.Port = Convert.ToInt32(ConfigurationManager.AppSettings["SmtpPort"]);
                client.EnableSsl = true;
                client.UseDefaultCredentials = false;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;

                client.Credentials = new NetworkCredential(
                    ConfigurationManager.AppSettings["Username"],
                    ConfigurationManager.AppSettings["Password"]);

                using (var message = new MailMessage())
                {
                    message.From = new MailAddress(
                        ConfigurationManager.AppSettings["SenderEmail"],
                        ConfigurationManager.AppSettings["SenderName"]);

                    message.To.Add(toEmail);

                    message.Subject = subject;

                    message.Body = body;

                    message.IsBodyHtml = true;

                    await client.SendMailAsync(message);
                }
            }
        }
    }
}