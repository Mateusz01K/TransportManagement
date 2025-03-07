using TransportManagement.Models.AssignTrailer;
using TransportManagement.Models.AssignTruck;
using TransportManagement.Models.Trailer;
using TransportManagement.Models.Truck;

namespace TransportManagement.Models.User
{
    public class UserProfileViewModel
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Role { get; set; }

        public List<AssignTruckModel> AssignedTrucks { get; set; } = new List<AssignTruckModel>();
        public List<TrailerModel> AssignedTrailers { get; set; } = new List<TrailerModel>();
    }
}
