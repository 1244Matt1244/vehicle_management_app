using AutoMapper;
using Moq;
using Project.Service.Interfaces;
using Project.Service.Models;
using Project.Service.Services;
using Xunit;
using Microsoft.AspNetCore.Mvc;  // For ViewResult
using Project.Service.Mappings;  // For ServiceMappingProfile
using Project.Service.Data.Helpers;    // For PaginatedList<>
using Project.MVC.ViewModels;  

namespace Project.Tests.UnitTests
{
    public class VehicleServiceTests
    {
        private readonly Mock<IVehicleRepository> _repositoryMock = null!;
        private readonly IMapper _mapper;
        private readonly VehicleService _service;

        public VehicleServiceTests()
        {
            _repositoryMock = new Mock<IVehicleRepository>();
            
            var config = new MapperConfiguration(cfg => 
                cfg.AddProfile<ServiceMappingProfile>());
            
            _mapper = config.CreateMapper();
            _service = new VehicleService(_repositoryMock.Object, _mapper);
        }

        [Fact]
        public async Task GetMakesAsync_ReturnsPaginatedList()
        {
            // Arrange
            var testData = TestHelpers.GetTestVehicleMakes();
            _repositoryMock.Setup(r => r.GetMakesAsync())
                .ReturnsAsync(testData);

            // Act
            var result = await _service.GetMakesAsync(1, 10, "Name", "", "");

            // Assert
            Assert.IsType<PaginatedList<VehicleMakeVM>>(result);
            Assert.Equal(2, result.Count);
        }
    }
}
