namespace TransportManagement.Models.Orders
{
    public class OrderViewModel
    {
        public OrderViewModel() { }
        public List<OrderModel> Orders { get; set; }
        public List<string> DriverEmails { get; set; }
    }
}
