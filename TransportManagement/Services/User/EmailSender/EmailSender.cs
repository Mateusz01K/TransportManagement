using System.Net;
using System.Net.Mail;

namespace TransportManagement.Services.User.EmailSender
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;
        private readonly string _uploadsFolder;
        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
            _uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            Directory.CreateDirectory(_uploadsFolder);
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

        public async Task<bool> SendCvAsync(string applicantEmail, IFormFile cvFile)
        {
            if (cvFile == null || cvFile.Length == 0)
            {
                return false;
            }

            var filePath = await SaveCvFileAsync(cvFile);
            if (string.IsNullOrEmpty(filePath))
                return false;

            try
            {
                var smtpSettings = _configuration.GetSection("EmailSettings");

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
                    Subject = "CV",
                    Body = $"W załączniku przesyłam swoje CV. Email: {applicantEmail}",
                    IsBodyHtml = false
                };

                mailMessage.To.Add(smtpSettings["SmtpUsername"]);
                mailMessage.Attachments.Add(new Attachment(filePath));

                //if (!string.IsNullOrEmpty(cvFilePath))
                //{
                //    mailMessage.Attachments.Add(new Attachment(cvFilePath));
                //}

                await client.SendMailAsync(mailMessage);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd wysyłania e-maiala:{ex.Message}");
                return false;
            }

        }

        public async Task<string> SaveCvFileAsync(IFormFile cvFile)
        {
            var filePath = Path.Combine(_uploadsFolder, cvFile.FileName);

            try
            {
                using var stream = new FileStream(filePath, FileMode.Create);
                await cvFile.CopyToAsync(stream);
                return filePath;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd zapisu pliku: {ex.Message}");
                return string.Empty;
            }
        }
    }
}
