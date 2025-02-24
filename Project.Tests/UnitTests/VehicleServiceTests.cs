// Project.Tests/UnitTests/VehicleServiceTests.cs
using AutoMapper;
using Moq;
using Project.Service.Data.DTOs;
using Project.Service.Interfaces;
using Project.Service.Models;
using Project.Service.Services;
using Project.Tests.Helpers;
using Xunit;

namespace Project.Tests.UnitTests
{
    public class VehicleServiceTests
    {
        private readonly Mock<IVehicleRepository> _repoMock = new();
        private readonly IMapper _mapper = TestHelpers.CreateTestMapper();

        [Fact]
        public async Task GetMakesAsync_ReturnsPaginatedList()
        {
            // Arrange
            var service = new VehicleService(_repoMock.Object, _mapper);
            var testData = await PaginatedList<VehicleMake>.CreateAsync(
                TestHelpers.GetTestMakes().AsQueryable(), 1, 10);
            
            _repoMock.Setup(r => r.GetMakesPaginatedAsync(It.IsAny<int>(), It.IsAny<int>(), 
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(testData);

            // Act
            var result = await service.GetMakesAsync(1, 10, "Name", "asc", "");

            // Assert
            Assert.IsType<PaginatedList<VehicleMakeDTO>>(result);
            Assert.Equal(2, result.Items.Count);
        }
    }
}