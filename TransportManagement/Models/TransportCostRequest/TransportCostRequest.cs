namespace TransportManagement.Models.TransportCostRequest
{
    public class TransportCostRequest
    {
        public string UserEmail { get; set; }
        public string PickupLocation { get; set; }
        public string DeliveryLocation { get; set; }
        public string Description { get; set; }
        public double Weight { get; set; }
        public double Width { get; set; }
        public double Length { get; set; }
        public double Height { get; set; }
        public int Quantity { get; set; }
        public TypeItem Type { get; set; }
        public IFormFile? File { get; set; }
    }


    public enum TypeItem
    {
        Paleta,
        Ciecz,
        Kruszywo
    }
}
