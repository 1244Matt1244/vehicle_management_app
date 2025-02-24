using Xunit;
using Moq;
using AutoMapper;
using Project.Service.Services;
using Project.Service.Interfaces;
using Project.Service.Data.DTOs;
using Project.Service.Data.Helpers;
using Project.Service.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore; // Ensure you have this using directive
using Project.Service.Mappings;

namespace Project.Tests.UnitTests
{
    public class VehicleServiceTests
    {
        private ApplicationDbContext _context;
        private IMapper _mapper;
        private IVehicleService _service;

        public VehicleServiceTests()
        {
            // Configure in-memory database
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite("Data Source=:memory:")
                .Options;

            _context = new ApplicationDbContext(options);
            _context.Database.OpenConnection();
            _context.Database.EnsureCreated();

            // Configure AutoMapper
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ServiceMappingProfile>();
            });
            _mapper = config.CreateMapper();

            _service = new VehicleService(_context, _mapper);
        }

        public void Cleanup()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Fact] // Use [Fact] instead of [TestMethod]
        public async Task GetMakesAsync_ReturnsPaginatedList()
        {
            // Add test data
            _context.VehicleMakes.AddRange(new[]
            {
                new VehicleMake { Name = "Make1" },
                new VehicleMake { Name = "Make2" }
            });
            await _context.SaveChangesAsync();

            // Act
            var result = await _service.GetMakesAsync(1, 10);

            // Assert
            Assert.Equal(2, result.TotalCount); // Use Assert.Equal for Xunit
        }
    }
}
