using Microsoft.EntityFrameworkCore;
using Project.Service.Models;

namespace Project.Service.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<VehicleMake> VehicleMakes { get; set; }
        public DbSet<VehicleModel> VehicleModels { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<VehicleModel>()
                .HasOne(vm => vm.VehicleMake)
                .WithMany(vm => vm.VehicleModels)
                .HasForeignKey(vm => vm.MakeId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
