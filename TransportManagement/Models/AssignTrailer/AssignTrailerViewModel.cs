using TransportManagement.Models.Trailer;
using TransportManagement.Models.Truck;

namespace TransportManagement.Models.AssignTrailer
{
    public class AssignTrailerViewModel
    {
        public AssignTrailerViewModel() { }
        public List<AssignTrailerModel> AssignTrailer { get; set; }
        public List<TruckModel> Truck { get; set; }
        public List<TrailerModel> Trailer { get; set; }
    }
}
