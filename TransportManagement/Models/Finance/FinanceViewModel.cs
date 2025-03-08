using TransportManagement.Models.FinanceReport;

namespace TransportManagement.Models.Finance
{
    public class FinanceViewModel
    {
        public FinanceViewModel() { }
        public List<FinanceModel> Finances { get; set; }
        public string EmployeeEmail { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal TotalExpenses { get; set; }
        public decimal TotalSalaries { get; set; }
        public decimal GrossProfit { get; set; }
        public decimal NetProfit { get; set; }
    }
}
