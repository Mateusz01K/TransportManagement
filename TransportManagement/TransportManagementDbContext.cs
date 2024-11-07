using Microsoft.EntityFrameworkCore;
using TransportManagement.Models.Drivers;

namespace TransportManagement
{
    public class TransportManagementDbContext : DbContext
    {
        public TransportManagementDbContext(DbContextOptions<TransportManagementDbContext> options) : base(options) { }

        public DbSet<DriverModel> Drivers { get; set; }
    }
}
