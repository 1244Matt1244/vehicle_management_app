// Project.Service/Data/VehicleContext.cs
using System.Data.Entity;
using Project.Service.Models;

namespace Project.Service.Data
{
    public class VehicleContext : DbContext
    {
        public VehicleContext() : base("VehicleDbConnection") { }

        public DbSet<VehicleMake> VehicleMakes { get; set; }
        public DbSet<VehicleModel> VehicleModels { get; set; }
    }
}
