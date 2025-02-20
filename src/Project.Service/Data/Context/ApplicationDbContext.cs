using Microsoft.EntityFrameworkCore;
using Project.Service.Shared.Models;
using Project.Service.Data;

namespace Project.Service.Data.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<VehicleMake> VehicleMakes { get; set; }
        public DbSet<VehicleModel> VehicleModels { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Correct relationship configuration
            modelBuilder.Entity<VehicleModel>()
                .HasOne(vm => vm.Make)          // Matches VehicleModel.Make navigation property
                .WithMany(vm => vm.Models)      // Matches VehicleMake.Models collection
                .HasForeignKey(vm => vm.MakeId)
                .OnDelete(DeleteBehavior.Cascade);

            ConfigureVehicleMakeEntity(modelBuilder);
            ConfigureVehicleModelEntity(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        private void ConfigureVehicleMakeEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<VehicleMake>()
                .HasKey(vm => vm.Id);
                
            modelBuilder.Entity<VehicleMake>()
                .Property(vm => vm.Name)
                .IsRequired()
                .HasMaxLength(50);
                
            modelBuilder.Entity<VehicleMake>()
                .Property(vm => vm.Abrv)
                .HasMaxLength(10);
        }

        private void ConfigureVehicleModelEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<VehicleModel>()
                .HasKey(vm => vm.Id);
                
            modelBuilder.Entity<VehicleModel>()
                .Property(vm => vm.Name)
                .IsRequired()
                .HasMaxLength(50);
                
            modelBuilder.Entity<VehicleModel>()
                .Property(vm => vm.Abrv)
                .HasMaxLength(10);
        }
    }
}
