using Microsoft.EntityFrameworkCore;
using Project.Service.Models;

namespace Project.Service.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        // DbSets for VehicleMake and VehicleModel
        public DbSet<VehicleMake> VehicleMakes { get; set; }
        public DbSet<VehicleModel> VehicleModels { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure VehicleModel -> VehicleMake relationship
            modelBuilder.Entity<VehicleModel>()
                .HasOne(vm => vm.VehicleMake)
                .WithMany(vm => vm.VehicleModels)
                .HasForeignKey(vm => vm.MakeId)
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete to remove related models when a make is deleted

            // Apply additional configurations if needed
            ConfigureVehicleMakeEntity(modelBuilder);
            ConfigureVehicleModelEntity(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        // Additional configuration for VehicleMake
        private void ConfigureVehicleMakeEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<VehicleMake>()
                .HasKey(vm => vm.Id); // Primary key
            modelBuilder.Entity<VehicleMake>()
                .Property(vm => vm.Name)
                .IsRequired()
                .HasMaxLength(50); // Name is required and has a max length of 50
            modelBuilder.Entity<VehicleMake>()
                .Property(vm => vm.Abrv)
                .HasMaxLength(10); // Abbreviation has a max length of 10
        }

        // Additional configuration for VehicleModel
        private void ConfigureVehicleModelEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<VehicleModel>()
                .HasKey(vm => vm.Id); // Primary key
            modelBuilder.Entity<VehicleModel>()
                .Property(vm => vm.Name)
                .IsRequired()
                .HasMaxLength(50); // Name is required and has a max length of 50
            modelBuilder.Entity<VehicleModel>()
                .Property(vm => vm.Abrv)
                .HasMaxLength(10); // Abbreviation has a max length of 10
        }
    }
}
