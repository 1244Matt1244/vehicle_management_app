using Microsoft.EntityFrameworkCore;
using Project.Service.Models;

namespace Project.Service.Data.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            // EF Core initializes these automatically
            VehicleMakes = Set<VehicleMake>();
            VehicleModels = Set<VehicleModel>();
        }

        public DbSet<VehicleMake> VehicleMakes { get; set; }
        public DbSet<VehicleModel> VehicleModels { get; set; }
    }
}