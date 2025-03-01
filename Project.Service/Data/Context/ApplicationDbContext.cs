using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection; 
using Project.Service.Models;

namespace Project.Service.Data.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<VehicleMake> VehicleMakes { get; set; }
        public DbSet<VehicleModel> VehicleModels { get; set; }

        // Static method to configure the in-memory database for testing
        public static void ConfigureForTests(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options => 
                options.UseInMemoryDatabase("TestDB"));
        }
    }
}
