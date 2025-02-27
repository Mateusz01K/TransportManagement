using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TransportManagement.Models.AssignTrailer;
using TransportManagement.Models.AssignTruck;
using TransportManagement.Models.Drivers;
using TransportManagement.Models.LeaveRequests;
using TransportManagement.Models.Orders;
using TransportManagement.Models.Trailer;
using TransportManagement.Models.Truck;
using TransportManagement.Models.User;

namespace TransportManagement
{
    public class TransportManagementDbContext : IdentityDbContext<ApplicationUser>//DbContext
    {
        public TransportManagementDbContext(DbContextOptions<TransportManagementDbContext> options) : base(options) { }

        public DbSet<DriverModel> Drivers { get; set; }
        public DbSet<TruckModel> Trucks { get; set; }
        public DbSet<TrailerModel> Trailers { get; set; }
        public DbSet<AssignTruckModel> AssignTrucks { get; set;}
        public DbSet<AssignTrailerModel> AssignTrailers { get; set; }
        public DbSet<OrderModel> Orders { get; set; }
        public DbSet<LeaveRequestModel> LeaveRequests { get; set; }
    }
}
