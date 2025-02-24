using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Project.Service.Services;
using Project.Service.Interfaces;
using Project.Service.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Project.Tests.UnitTests
{
    [TestClass]
    public class ServiceTests
    {
        private Mock<IVehicleRepository> _mockRepo = null!;
        private VehicleService _service = null!;

        [TestInitialize]
        public void Setup()
        {
            _mockRepo = new Mock<IVehicleRepository>();
            _service = new VehicleService(_mockRepo.Object);
        }

        [TestMethod]
        public async Task GetMakesAsync_ReturnsPagedList()
        {
            // Arrange
            var makes = new List<VehicleMake>
            {
                new VehicleMake { Id = 1, Name = "TestMake1", Abrv = "TM1" },
                new VehicleMake { Id = 2, Name = "TestMake2", Abrv = "TM2" }
            };

            _mockRepo.Setup(repo => repo.GetMakesAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(makes);

            // Act
            var result = await _service.GetMakesAsync(1, 10, "Name", "asc", "");

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
        }
    }
}