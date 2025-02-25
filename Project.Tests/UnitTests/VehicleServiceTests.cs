using Xunit;
using Moq;
using Project.Service.Interfaces;
using Project.Service.Models;
using Project.Service.Services;
using Project.Tests;
using System.Threading.Tasks;

namespace Project.Tests.UnitTests
{
    public class VehicleServiceTests
    {
        private readonly Mock<IVehicleRepository> _mockRepo;
        private readonly VehicleService _service;

        public VehicleServiceTests()
        {
            _mockRepo = new Mock<IVehicleRepository>();
            _service = new VehicleService(_mockRepo.Object, TestHelpers.CreateTestMapper());
        }

        [Fact]
        public async Task GetMakeById_ReturnsMake()
        {
            // Arrange
            var testMake = TestHelpers.CreateTestMake();
            _mockRepo.Setup(r => r.GetMakeByIdAsync(1)).ReturnsAsync(testMake);

            // Act
            var result = await _service.GetMakeByIdAsync(1);

            // Assert
            Assert.Equal("Make1", result.Name);
            Assert.Equal("M1", result.Abbreviation);
        }
    }
}