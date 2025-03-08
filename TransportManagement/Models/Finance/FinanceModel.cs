namespace TransportManagement.Models.Finance
{
    public class FinanceModel
    {
        public int Id { get; set; }
        public string Description { get; set; } = "Brak";
        public decimal Amount { get; set; }
        public FinanceType Type { get; set; }
        public DateTime Date { get; set; }
        public string? EmployeeEmail { get; set; }
    }

    public enum FinanceType
    {
        Przychód,
        Wydatek,
        Pensja
    }
}
