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
            // Configure VehicleMake
            modelBuilder.Entity<VehicleMake>()
                .Property(vm => vm.Name)
                .IsRequired()
                .HasMaxLength(100);

            // Configure VehicleModel
            modelBuilder.Entity<VehicleModel>()
                .Property(vm => vm.Name)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<VehicleModel>()
                .HasOne(vm => vm.VehicleMake)
                .WithMany(v => v.VehicleModels) // One-to-many relationship
                .HasForeignKey(vm => vm.VehicleMakeId)
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete when parent is removed
        }
    }
}
