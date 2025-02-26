using Microsoft.EntityFrameworkCore;
using Project.Service.Data.Context;
using Project.Service.Models;
using Project.Service.Repositories;
using Project.Tests;
using Xunit;

namespace Project.Tests.UnitTests
{
    public class VehicleServiceTests
    {
        private readonly ApplicationDbContext _context;
        private readonly VehicleRepository _repository;

        public VehicleServiceTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: $"TestDb_{Guid.NewGuid()}")
                .Options;

            _context = new ApplicationDbContext(options);
            _repository = new VehicleRepository(_context);
        }

        [Fact]
        public async Task CreateMake_AddsNewVehicleMake()
        {
            // Arrange
            var testMake = TestHelpers.CreateTestMake();

            // Act
            await _repository.CreateMakeAsync(testMake);
            await _context.SaveChangesAsync();

            // Assert
            Assert.Equal(1, await _context.VehicleMakes.CountAsync());
        }
    }
}