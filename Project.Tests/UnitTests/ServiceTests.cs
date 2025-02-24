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

namespace Project.Tests.UnitTests
{
    public class VehicleServiceTests
    {
        private readonly IVehicleService _service;
        private readonly IMapper _mapper;

        public VehicleServiceTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ServiceMappingProfile>();
            });
            _mapper = config.CreateMapper();

            var mockRepository = new Mock<IVehicleRepository>();
            // Setup mock repository methods as needed

            _service = new VehicleService(mockRepository.Object, _mapper);
        }

        [Fact]
        public async Task GetMakesAsync_ReturnsPaginatedList()
        {
            // Add test data to mock repository
            var makes = new List<VehicleMake>
            {
                new VehicleMake { Id = 1, Abrv = "VM1", Name = "Make1" },
                new VehicleMake { Id = 2, Abrv = "VM2", Name = "Make2" }
            };

            // Mock repository behavior
            mockRepository.Setup(repo => repo.GetMakesAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new PaginatedList<VehicleMakeDTO>(
                    makes.Select(m => new VehicleMakeDTO { Id = m.Id, Abrv = m.Abrv, Name = m.Name }).ToList(),
                    makes.Count, 1, 10));

            // Act
            var result = await _service.GetMakesAsync(1, 10, "name", null, null);

            // Assert
            Assert.Equal(2, result.TotalCount);
        }
    }
}
