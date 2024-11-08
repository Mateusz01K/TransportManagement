namespace TransportManagement.Models.Truck
{
    public class TruckModel
    {
        public TruckModel() { }
        public int Id { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int YearOfProduction { get; set; }
        public string Power { get; set; }
        public float Mileage { get; set; }
        public int Weight { get; set; }
        public string LicensePlate { get; set; }
        public bool IsAssignedDriver { get; set; }
        public bool IsAssignedTrailer { get; set; }
    }
}
