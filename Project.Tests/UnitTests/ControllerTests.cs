// At the top of ControllerTests.cs
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xunit;
using Moq;
using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Project.MVC.Controllers;
using Project.Service.Models;
using Project.Service.Interfaces;
using Project.Service.Data.DTOs;
using Project.Service.Data.Helpers;
using Project.Service.Data.Context;  

using Project.Service.Mappings;

namespace Project.Tests.UnitTests.ControllerTests
{
    public class VehicleMakeControllerTests
    {
        private readonly VehicleMakeController _controller;
        private readonly IMapper _mapper;

        public VehicleMakeControllerTests()
        {
            // Configure AutoMapper
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ServiceMappingProfile>();
                cfg.AddProfile<MvcMappingProfile>();
            });
            _mapper = config.CreateMapper();

            var mockService = new Mock<IVehicleService>();
            mockService.Setup(s => s.GetMakesAsync(It.IsAny<int>(), It.IsAny<int>(), null, null, null))
                .ReturnsAsync(new PaginatedList<VehicleMakeDTO>(
                    new List<VehicleMakeDTO> { new VehicleMakeDTO { Name = "Test" } },
                    1, 1, 10));

            _controller = new VehicleMakeController(mockService.Object, _mapper);
        }

        [Fact] // Use [Fact] instead of [TestMethod]
        public async Task Index_ReturnsViewWithMakes()
        {
            // Act
            var result = await _controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result); // Assert that result is of type ViewResult
            Assert.NotNull(viewResult); // Check that the view result is not null
        }
    }
}
