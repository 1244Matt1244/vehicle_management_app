using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Project.Service.Models;

namespace Project.Service.Data.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<VehicleMake> VehicleMakes { get; set; }
        public DbSet<VehicleModel> VehicleModels { get; set; }

        // Static method to configure the in-memory database for testing
        public static void ConfigureForTests(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options => 
                options.UseInMemoryDatabase("TestDB"));
        }

        // Override OnModelCreating to configure entity relationships and constraints
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Example configuration for the VehicleMake entity
            modelBuilder.Entity<VehicleMake>()
                .HasKey(vm => vm.Id); // Set the primary key for VehicleMake

            modelBuilder.Entity<VehicleMake>()
                .Property(vm => vm.Name)
                .IsRequired() // Mark Name as required
                .HasMaxLength(100); // Set maximum length for Name

            // Example configuration for the VehicleModel entity
            modelBuilder.Entity<VehicleModel>()
                .HasKey(vm => vm.Id); // Set the primary key for VehicleModel

            modelBuilder.Entity<VehicleModel>()
                .Property(vm => vm.Name)
                .IsRequired() // Mark Name as required
                .HasMaxLength(100); // Set maximum length for Name

            modelBuilder.Entity<VehicleModel>()
                .HasOne(vm => vm.VehicleMake) // Set up the one-to-many relationship with VehicleMake
                .WithMany(v => v.VehicleModels) // The VehicleMake has many VehicleModels
                .HasForeignKey(vm => vm.VehicleMakeId); // The foreign key is VehicleMakeId
        }
    }
}
