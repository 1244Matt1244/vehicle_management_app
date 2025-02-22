using Microsoft.EntityFrameworkCore;
using Project.Service.Models;

namespace Project.Service.Data.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<VehicleMake> VehicleMakes { get; set; }
        public DbSet<VehicleModel> VehicleModels { get; set; }

        // Optional: Add fluent API configurations for relationships/constraints
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure VehicleModel -> VehicleMake relationship
            modelBuilder.Entity<VehicleModel>()
                .HasOne(vm => vm.VehicleMake)
                .WithMany(vm => vm.VehicleModels)
                .HasForeignKey(vm => vm.MakeId)
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete (optional)

            // Seed initial data (optional)
            modelBuilder.Entity<VehicleMake>().HasData(
                new VehicleMake { Id = 1, Name = "BMW", Abrv = "B" },
                new VehicleMake { Id = 2, Name = "Ford", Abrv = "F" }
            );
        }
    }
}