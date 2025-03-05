namespace TransportManagement.Models.FinanceReport
{
    public class FinanceReportModel
    {
        public int Id { get; set; }
        public string DriverEmail { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal TotalExpenses { get; set; }
        public decimal TotalSalary { get; set; }
        public decimal TotalProfitFromCompletedOrders { get; set; }
        public decimal NetProfit => TotalRevenue - TotalExpenses + TotalProfitFromCompletedOrders;
    }
}
