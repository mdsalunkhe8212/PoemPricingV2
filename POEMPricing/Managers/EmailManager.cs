using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System.Configuration;
using System.Threading.Tasks;

namespace POEMPricing.Managers
{
    public class EmailManager
    {
        private readonly string _host;
        private readonly int _port;
        private readonly string _senderName;
        private readonly string _senderEmail;
        private readonly string _username;
        private readonly string _password;

        public EmailManager()
        {
            _host = ConfigurationManager.AppSettings["SmtpServer"];
            _port = int.Parse(ConfigurationManager.AppSettings["SmtpPort"]);
            _senderName = ConfigurationManager.AppSettings["SenderName"];
            _senderEmail = ConfigurationManager.AppSettings["SenderEmail"];
            _username = ConfigurationManager.AppSettings["Username"];
            _password = ConfigurationManager.AppSettings["Password"];
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var email = new MimeMessage();

            email.From.Add(new MailboxAddress(_senderName, _senderEmail));

            email.To.Add(MailboxAddress.Parse(toEmail));

            email.Subject = subject;

            email.Body = new TextPart("html")
            {
                Text = body
            };

            using (var smtp = new SmtpClient())
            {
                await smtp.ConnectAsync(
                    _host,
                    _port,
                    SecureSocketOptions.SslOnConnect);

                await smtp.AuthenticateAsync(
                    _username,
                    _password);

                await smtp.SendAsync(email);

                await smtp.DisconnectAsync(true);
            }
        }
    }
}