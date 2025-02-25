using Project.Service.Data.Context;
using Project.Service.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Project.Tests.IntegrationTests
{
    public class DatabaseTests
    {
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

                var model = new VehicleModel { Id = 1, Name = "Model", Abbreviation = "M", MakeId = 1 };
                context.VehicleModels.Add(model);
                context.SaveChanges();

                Assert.NotNull(context.VehicleMakes.Find(1));
                Assert.NotNull(context.VehicleModels.Find(1));
            }
        }
    }
}