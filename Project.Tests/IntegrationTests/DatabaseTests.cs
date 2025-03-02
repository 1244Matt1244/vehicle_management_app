using Project.Service.Data.Context;
using Project.Service.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using System.Linq;

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
                    var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
                    if (descriptor != null)
                    {
                        services.Remove(descriptor);
                    }

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
