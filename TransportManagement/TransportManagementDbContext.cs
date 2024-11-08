using Microsoft.EntityFrameworkCore;
using TransportManagement.Models.Drivers;
using TransportManagement.Models.Trailer;
using TransportManagement.Models.Truck;

namespace TransportManagement
{
    public class TransportManagementDbContext : DbContext
    {
        public TransportManagementDbContext(DbContextOptions<TransportManagementDbContext> options) : base(options) { }

        public DbSet<DriverModel> Drivers { get; set; }
        public DbSet<TruckModel> Trucks { get; set; }
        public DbSet<TrailerModel> Trailers { get; set; }
    }
}
