using Project.Service.Data.Context;
using Project.Service.Models;
using Project.Service.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Project.Tests.UnitTests
{
    public class VehicleRepositoryTests
    {
        private readonly DbContextOptions<ApplicationDbContext> _options;

        public VehicleRepositoryTests()
        {
            _options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;
        }

        [Fact]
        public async Task CreateMake_AddsNewVehicleMake()
        {
            // Arrange
            var testMake = new VehicleMake { Id = 1, Name = "TestMake", Abbreviation = "TM" };
            
            using (var context = new ApplicationDbContext(_options))
            {
                var repository = new VehicleRepository(context);

                // Act
                await repository.CreateMakeAsync(testMake);
                await context.SaveChangesAsync();
            }

            // Assert
            using (var context = new ApplicationDbContext(_options))
            {
                Assert.Equal(1, await context.VehicleMakes.CountAsync());
            }
        }
    }
}