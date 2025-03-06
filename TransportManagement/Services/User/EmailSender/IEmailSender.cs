using TransportManagement.Models.TransportCostRequest;

namespace TransportManagement.Services.User.EmailSender
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
        Task<bool> SendCvAsync(string applicantEmail, IFormFile cvFile);
        Task<string> SaveCvFileAsync(IFormFile cvFile);
        Task<bool> SendTransportCostRequestEmailAsync(string userEmail, string pickupLocation, string deliveryLocation, string description,
            double weight, double width, double length, double height, int quantity, TypeItem type, IFormFile file);
        Task<string> SaveFileAsync(IFormFile file);
    }
}