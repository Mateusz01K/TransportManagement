namespace TransportManagement.Models.LeaveRequests
{
    public class LeaveRequestModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public LeaveStatus Status { get; set; } = LeaveStatus.Oczekujące;
        public string? AdminComment { get; set; }
        public DateTime RequestDate { get; set; }
    }
    public enum LeaveStatus
    {
        Oczekujące,
        Zaakceptowane,
        Odrzucone
    }
}
