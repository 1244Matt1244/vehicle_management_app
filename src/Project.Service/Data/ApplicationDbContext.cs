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
            modelBuilder.Entity<VehicleMake>().HasKey(m => m.Id);
            modelBuilder.Entity<VehicleModel>().HasKey(m => m.Id);

            // Configure relationships
            modelBuilder.Entity<VehicleModel>()
                .HasOne(vm => vm.Make)
                .WithMany()
                .HasForeignKey(vm => vm.MakeId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}