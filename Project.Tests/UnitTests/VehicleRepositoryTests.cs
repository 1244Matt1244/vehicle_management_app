using Microsoft.EntityFrameworkCore;
using Moq;
using Project.Service.Data.Context;
using Project.Service.Data.Helpers;
using Project.Service.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Project.Tests.UnitTests
{
    public class VehicleRepositoryTests
    {
        private readonly DbContextOptions<ApplicationDbContext> _options;

        public VehicleRepositoryTests()
        {
            _options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
        }

        [Fact]
        public async Task GetMakesPaginatedAsync_ReturnsCorrectResults()
        {
            // Arrange
            using var context = new ApplicationDbContext(_options);
            context.VehicleMakes.AddRange(new[]
            {
                new VehicleMake { Name = "Test1", Abrv = "T1" },
                new VehicleMake { Name = "Test2", Abrv = "T2" }
            });
            await context.SaveChangesAsync();

            var repository = new VehicleRepository(context);

            // Act
            var result = await repository.GetMakesPaginatedAsync(1, 10, "Name", "asc", "");

            // Assert
            Assert.Equal(2, result.Makes.Count);
            Assert.Equal("Test1", result.Makes.First().Name);
        }
    }
}