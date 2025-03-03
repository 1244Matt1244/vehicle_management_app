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

        /// <summary>
        /// Configures an in-memory database for testing.
        /// </summary>
        public static void ConfigureForTesting(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase("TestDB"));
        }

        /// <summary>
        /// Configures entity relationships and constraints.
        /// </summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure VehicleMake entity
            modelBuilder.Entity<VehicleMake>()
                .Property(vm => vm.Name)
                .IsRequired()
                .HasMaxLength(100);

            // Configure VehicleModel entity
            modelBuilder.Entity<VehicleModel>()
                .Property(vm => vm.Name)
                .IsRequired()
                .HasMaxLength(100);

            // Fix Foreign Key Configuration for VehicleModel and VehicleMake
            modelBuilder.Entity<VehicleModel>()
                .HasOne(vm => vm.VehicleMake) // Correct the navigation property
                .WithMany(m=> m.VehicleModels) // Correct the navigation property on the other side
                .HasForeignKey(vm => vm.VehicleMakeId) // Ensure this is the correct property name
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete when parent is removed

            // Add explicit configuration to avoid lazy-loading issues
            modelBuilder.Entity<VehicleModel>()
                .HasOne(vm => vm.VehicleMake)
                .WithMany()
                .HasForeignKey(vm => vm.VehicleMakeId)
                .OnDelete(DeleteBehavior.Restrict); // Restrict delete behavior for prevention of unintended cascading

            base.OnModelCreating(modelBuilder);
        }
    }
}
