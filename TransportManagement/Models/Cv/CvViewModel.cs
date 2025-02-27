using System.ComponentModel.DataAnnotations;

namespace TransportManagement.Models.Cv
{
    public class CvViewModel
    {
        //[Required(ErrorMessage = "Adres e-mail jest wymagany.")]
        //[EmailAddress(ErrorMessage = "Podaj prawidłowy adres e-mail.")]
        public string ApplicantEmail { get; set; }
        //[Required(ErrorMessage = "Plik CV jest wymagany.")]
        public IFormFile CvFile { get; set; }
    }
}
