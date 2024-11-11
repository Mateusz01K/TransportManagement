using TransportManagement.Models.Trailer;
using TransportManagement.Models.Truck;

namespace TransportManagement.Models.AssignTrailer
{
    public class AssignTrailerModel
    {
        public AssignTrailerModel() { }
        public int Id { get; set; }
        public int TruckId { get; set; }
        public TruckModel Truck { get; set; }
        public int TrailerId { get; set; }
        public TrailerModel Trailer { get; set; }
        public DateTime AssignmentDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public bool IsReturned { get; set; }
    }
}
