using Microsoft.AspNetCore.Mvc;
using TransportManagement.Models.TransportCostRequest;
using TransportManagement.Services.User.EmailSender;

namespace TransportManagement.Controllers
{
    public class TransportRequestController : Controller
    {
        private readonly IEmailSender _emailSender;

        public TransportRequestController(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }

        [HttpGet]
        public IActionResult RequestTransportCost()
        {
            return View(new TransportCostRequest());
        }

        [HttpPost]
        public async Task<IActionResult> RequestTransportCost([FromForm] TransportCostRequest model)
        {
            if (model == null)
            {
                TempData["message"] = "Proszę wypełnić formularz.";
                return View(model);
            }
            var success = await _emailSender.SendTransportCostRequestEmailAsync(model.UserEmail,model.PickupLocation,model.DeliveryLocation,
                model.Description,model.Weight,model.Width,model.Length,model.Height,model.Quantity,model.Type,model.File);
            if (!success)
            {
                TempData["message"] = "Wystąpił błąd podczas wysyłania zapytania.";
                return View(model);
            }
            TempData["message"] = "Zapytanie o koszt transportu zostało wysłane pomyślnie.";
            return RedirectToAction("RequestTransportCost");
        }
    }
}
