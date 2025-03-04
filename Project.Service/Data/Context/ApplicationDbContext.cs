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

        public static void ConfigureForTesting(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase("TestDB"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // VehicleMake Configuration
            modelBuilder.Entity<VehicleMake>(entity =>
            {
                entity.Property(vm => vm.Name)
                    .IsRequired()
                    .HasMaxLength(100);
                
                entity.Property(vm => vm.Abbreviation)
                    .IsRequired()
                    .HasMaxLength(10);
            });

            // VehicleModel Configuration
            modelBuilder.Entity<VehicleModel>(entity =>
            {
                entity.Property(vm => vm.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(vm => vm.Abbreviation)
                    .IsRequired()
                    .HasMaxLength(10);

                // Single Relationship Configuration
                entity.HasOne(vm => vm.VehicleMake)
                    .WithMany(m => m.VehicleModels)
                    .HasForeignKey(vm => vm.VehicleMakeId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}