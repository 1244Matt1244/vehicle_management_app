// Project.Tests/UnitTests/ControllerTests.cs
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Project.MVC.Controllers;
using Project.Service.Interfaces;
using Project.Service.Data.DTOs;
using Project.Service.Data.Helpers;
using AutoMapper;
using Project.MVC.Mappings;
using Project.Service.Mappings;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Project.Tests.UnitTests.ControllerTests
{
    [TestClass]
    public class VehicleMakeControllerTests
    {
        private VehicleMakeController _controller;
        private IMapper _mapper;

        [TestInitialize]
        public void Setup()
        {
            // AutoMapper configuration
            var config = new MapperConfiguration(cfg => 
            {
                cfg.AddProfile<ServiceMappingProfile>();
                cfg.AddProfile<MvcMappingProfile>();
            });
            _mapper = config.CreateMapper();

            // Mock service
            var mockService = new Mock<IVehicleService>();
            mockService.Setup(s => s.GetMakesAsync(1, 10, null, null, null))
                .ReturnsAsync(new PaginatedList<VehicleMakeDTO>(
                    new List<VehicleMakeDTO> { new VehicleMakeDTO { Name = "Test" } }, 
                    1, 1, 10));

            _controller = new VehicleMakeController(mockService.Object, _mapper);
        }

        [TestMethod]
        public async Task Index_ReturnsViewWithMakes()
        {
            // Act
            var result = await _controller.Index();

            // Assert
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsNotNull(result);
        }
    }
}