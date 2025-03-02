using Project.Service.Data.Context;
using Project.Service.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Project.Tests.IntegrationTests
{
    public class DatabaseTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public DatabaseTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory.WithWebHostBuilder(builder => 
            {
                builder.ConfigureServices(services => 
                {
                    // Remove existing database configuration
                    services.RemoveAll<DbContextOptions<ApplicationDbContext>>();

                    // Add in-memory database with a unique name for each test
                    services.AddDbContext<ApplicationDbContext>(options => 
                        options.UseInMemoryDatabase($"TestDB_{Guid.NewGuid()}"));
                });
            });
        }

        [Fact]
        public void Database_ShouldCreateVehicleMakeWithModels()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            // Act & Assert
            using (var context = new ApplicationDbContext(options))
            {
                var make = new VehicleMake { Id = 1, Name = "Test", Abbreviation = "T" };
                context.VehicleMakes.Add(make);
                context.SaveChanges();

                var model = new VehicleModel { Id = 1, Name = "Model", Abbreviation = "M", VehicleMakeId = 1 };
                context.VehicleModels.Add(model);
                context.SaveChanges();

                Assert.NotNull(context.VehicleMakes.Find(1));
                Assert.NotNull(context.VehicleModels.Find(1));
            }
        }
    }
}
