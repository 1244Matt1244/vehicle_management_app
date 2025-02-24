// Project.Tests/UnitTests/ServiceTests.cs
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Project.Service.Data.Context;
using Project.Service.Interfaces;
using Project.Service.Models;
using Project.Service.Services;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Project.Service.Mappings;

namespace Project.Tests.UnitTests.ServiceTests
{
    [TestClass]
    public class VehicleServiceTests
    {
        private ApplicationDbContext _context;
        private IVehicleService _service;
        private IMapper _mapper;

        [TestInitialize]
        public void Setup()
        {
            // Configure in-memory database
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            _context = new ApplicationDbContext(options);
            
            // Configure AutoMapper
            var config = new MapperConfiguration(cfg => cfg.AddProfile<ServiceMappingProfile>());
            _mapper = config.CreateMapper();

            var repository = new VehicleRepository(_context);
            _service = new VehicleService(repository, _mapper);
        }

        [TestMethod]
        public async Task GetMakesAsync_ReturnsPaginatedList()
        {
            // Arrange
            _context.VehicleMakes.AddRange(new[]
            {
                new VehicleMake { Name = "BMW", Abrv = "B" },
                new VehicleMake { Name = "Ford", Abrv = "F" }
            });
            await _context.SaveChangesAsync();

            // Act
            var result = await _service.GetMakesAsync(1, 10);

            // Assert
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(2, result.TotalCount);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}