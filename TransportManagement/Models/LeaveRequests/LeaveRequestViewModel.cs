namespace TransportManagement.Models.LeaveRequests
{
    public class LeaveRequestViewModel
    {
        public LeaveRequestViewModel() { }
        public List<LeaveRequestModel> LeaveRequests { get; set; }
        public Dictionary<string, string> UsersEmails { get; set; }
    }
}
