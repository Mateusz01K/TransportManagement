namespace TransportManagement.Services.User.EmailSender
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
        Task<bool> SendCvAsync(string applicantEmail, IFormFile cvFile);
        Task<string> SaveCvFileAsync(IFormFile cvFile);
    }
}