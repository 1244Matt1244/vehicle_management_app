using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using Project.Service.Data;
using Project.Service.Data.Context;
using Project.Service.Interfaces;
using Project.Service.Models;
using Project.Service.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Project.Tests.Services
{
    public class VehicleServiceTests
    {
        private readonly IVehicleService _vehicleService;
        private readonly Mock<ApplicationDbContext> _dbContextMock;
        private readonly IMapper _mapper;

        public VehicleServiceTests()
        {
            _dbContextMock = new Mock<ApplicationDbContext>();
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ServiceMappingProfile>();
            });
            _mapper = mapperConfig.CreateMapper();

            _vehicleService = new VehicleService(_dbContextMock.Object);
        }

        [Fact]
        public async Task GetAllVehicleModelsAsync_ReturnsAllModels()
        {
            // Arrange
            var vehicleModels = new List<VehicleModel>
            {
                new VehicleModel { Id = 1, Name = "Model1" },
                new VehicleModel { Id = 2, Name = "Model2" }
            }.AsQueryable();

            var dbSetMock = new Mock<DbSet<VehicleModel>>();
            dbSetMock.As<IQueryable<VehicleModel>>().Setup(m => m.Provider).Returns(vehicleModels.Provider);
            dbSetMock.As<IQueryable<VehicleModel>>().Setup(m => m.Expression).Returns(vehicleModels.Expression);
            dbSetMock.As<IQueryable<VehicleModel>>().Setup(m => m.ElementType).Returns(vehicleModels.ElementType);
            dbSetMock.As<IQueryable<VehicleModel>>().Setup(m => m.GetEnumerator()).Returns(vehicleModels.GetEnumerator());

            _dbContextMock.Setup(c => c.VehicleModels).Returns(dbSetMock.Object);

            // Act
            var result = await _vehicleService.GetAllVehicleModelsAsync();

            // Assert
            Assert.Equal(2, result.Count());
        }
    }
}
