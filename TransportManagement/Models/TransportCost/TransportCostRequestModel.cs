namespace TransportManagement.Models.TransportCost
{
    public class TransportCostRequestModel
    {
        public double Distance { get; set; }
        public double FuelConsumption { get; set; }
        public double FuelPrice { get; set; }
        public double FreightPrice { get; set; }
        public string Currency { get; set; }
    }
}
