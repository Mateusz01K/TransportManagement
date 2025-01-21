namespace TransportManagement.Models.Cargo
{
    public class CargoModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public double Weight { get; set; }
        public int Width { get; set; }
        public int Lenght { get; set; }
        public int Height { get; set; }
        public int Quantity { get; set; }
        public string Type { get; set; }
    }
}
