using Microsoft.AspNetCore.Mvc;
using TransportManagement.Models.Cv;
using TransportManagement.Services.User.EmailSender;

namespace TransportManagement.Controllers
{
    public class CvController : Controller
    {
        private readonly IEmailSender _emailSender;


        public CvController(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }

        [HttpGet]
        public IActionResult SendCv()
        {
            return View(new CvViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> SendCv([FromForm] CvViewModel model)
        {
            if(model.CvFile == null || model.CvFile.Length == 0)
            {
                TempData["message"] = "Nie wybrano pliku CV.";
                return View(model);
            }

            var success = await _emailSender.SendCvAsync(model.ApplicantEmail, model.CvFile);
            if (!success)
            {
                TempData["message"] = "Wystąpił błąd podczas wysyłania CV.";
                return View(model);
            }

            TempData["message"] = "CV zostało wysłane pomyślnie.";
            return View(model);
        }
    }
}
