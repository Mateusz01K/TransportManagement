using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Text.Json;
using System.Text.Json.Serialization;
using TransportManagement.Models.TransportCost;

namespace TransportManagement.Controllers
{
    public class TransportCostController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;


        public TransportCostController(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> TransportCost()
        {
            return View(new TransportCostRequestViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> CalculateCost(/*[FromBody]*/ TransportCostRequestModel request)
        {
            if (request == null)
            {
                return BadRequest("Nieprawidłowe dane.");
            }

            double fuelCost = (request.Distance / 100) * request.FuelConsumption * request.FuelPrice;
            double profit = request.FreightPrice - fuelCost;

            double exchangeRate = await GetExchangeRate(request.Currency);
            Dictionary<string, double> allRates = await GetAllExchangeRates();

            return Ok(new TransportCostResponseModel
            {
                TotalCost = Math.Round(fuelCost * exchangeRate, 2),
                Profit = Math.Round(profit * exchangeRate, 2),
                Currency = request.Currency,
                ExchangeRates = allRates
            });
        }

        public async Task<IActionResult> GetExchangeRates()
        {
            string apiKey = _configuration["CurrencyConverter:ApiKey"];
            string apiUrl = $"https://api.currencyapi.com/v3/latest?apikey={apiKey}";
            using HttpClient client = new HttpClient();
            var response = await client.GetAsync(apiUrl);

            if (!response.IsSuccessStatusCode)
                return BadRequest("Nie udało się pobrać kursow walut.");


            var jsonResponse = await response.Content.ReadAsStringAsync();
            return Ok(JsonSerializer.Deserialize<object>(jsonResponse));
            //var exchangeRates = await GetAllExchangeRates();
            //return Ok(exchangeRates);
        }

        private async Task<double> GetExchangeRate(string currency)
        {
            string apiKey = _configuration["CurrencyConverter:ApiKey"];
            string apiUrl = $"https://api.currencyapi.com/v3/latest?apikey={apiKey}&currencies={currency}";

            var requestMessage = new HttpRequestMessage(HttpMethod.Get, apiUrl);
            requestMessage.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            var response = await _httpClient.SendAsync(requestMessage);
            if (!response.IsSuccessStatusCode)
                return 1.0;

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var jsonDocument = JsonDocument.Parse(jsonResponse);

            if (jsonDocument.RootElement.TryGetProperty("data", out var dataElement) && dataElement.TryGetProperty(currency, out var currencyData)
                && currencyData.TryGetProperty("value", out var rateValue))
            {
                return rateValue.GetDouble();
            }

            return 1.0;
        }

        private async Task<Dictionary<string, double>> GetAllExchangeRates()
        {
            string apiKey = _configuration["CurrencyConverter:ApiKey"];
            string apiUrl = $"https://api.currencyapi.com/v3/latest?apikey={apiKey}";

            var requestMessage = new HttpRequestMessage(HttpMethod.Get, apiUrl);
            requestMessage.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            var response = await _httpClient.SendAsync(requestMessage);
            if (!response.IsSuccessStatusCode)
                return new Dictionary<string, double>();

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var jsonDocument = JsonDocument.Parse(jsonResponse);

            var exchangeRates = new Dictionary<string, double>();

            if (jsonDocument.RootElement.TryGetProperty("data", out var dataElement))
            {
                foreach (var property in dataElement.EnumerateObject())
                {
                    //    exchangeRates.Add(property.Name, property.Value.GetProperty("value").GetDouble());

                    if (property.Value.TryGetProperty("value", out var valueElement) && valueElement.ValueKind == JsonValueKind.Number)
                    {
                        exchangeRates.Add(property.Name, valueElement.GetDouble());
                    }
                }
            }
        

            return exchangeRates;
        }
}
}
