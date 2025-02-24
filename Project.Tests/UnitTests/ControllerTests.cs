using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using AutoMapper;
using Project.MVC.Controllers;
using Project.Service.Services;
using Project.Service.Interfaces;
using Project.Service.Data.DTOs;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace Project.Tests.UnitTests
{
    [TestClass]
    public class ControllerTests
    {
        private VehicleMakeController _controller = null!;
        private Mock<IVehicleService> _mockService = null!;
        private IMapper _mapper = null!;

        [TestInitialize]
        public void Setup()
        {
            var config = new MapperConfiguration(cfg => 
                cfg.AddProfile<ServiceMappingProfile>());
            _mapper = config.CreateMapper();
            
            _mockService = new Mock<IVehicleService>();
            _controller = new VehicleMakeController(_mockService.Object, _mapper);
        }

        [TestMethod]
        public void GetMakes_ReturnsOkResult()
        {
            // Arrange
            var makes = new List<VehicleMakeDto>
            {
                new VehicleMakeDto { Id = 1, Name = "Test1", Abrv = "T1" },
                new VehicleMakeDto { Id = 2, Name = "Test2", Abrv = "T2" }
            };

            _mockService.Setup(s => s.GetMakesAsync(1, 10, "Name", "asc", ""))
                .ReturnsAsync(makes);

            // Act
            var result = _controller.GetMakes(1, 10, "Name", "asc", "").Result as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
        }
    }
}