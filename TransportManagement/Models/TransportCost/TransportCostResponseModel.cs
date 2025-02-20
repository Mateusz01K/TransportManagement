namespace TransportManagement.Models.TransportCost
{
    public class TransportCostResponseModel
    {
        public double TotalCost { get; set; }
        public double Profit { get; set; }
        public string Currency { get; set; }
        public Dictionary<string, double> ExchangeRates { get; set; }
    }
}
