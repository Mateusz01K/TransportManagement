using System.Net;
using System.Net.Mail;

namespace TransportManagement.Services.User.EmailSender
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;
        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var smtpSettings = _configuration.GetSection("EmailSettings");

            try
            {
                Console.WriteLine($"Adres e-mail: {email}");
                using var client = new SmtpClient
                {
                    Host = smtpSettings["SmtpServer"],
                    Port = int.Parse(smtpSettings["SmtpPort"]),
                    Credentials = new NetworkCredential(smtpSettings["SmtpUsername"], smtpSettings["SmtpPassword"]),
                    EnableSsl = true
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(smtpSettings["FromEmail"]),
                    Subject = subject,
                    Body = message,
                    IsBodyHtml = true
                };
                mailMessage.To.Add(email);

                await client.SendMailAsync(mailMessage);

                Console.WriteLine("E-mail został wysłany.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd wysyłania e-maiala:{ex.Message}");
                throw;
            }
            
        }
    }
}
