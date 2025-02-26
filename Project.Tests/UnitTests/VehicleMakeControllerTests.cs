using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Project.MVC.Controllers;
using Project.MVC.ViewModels;
using Project.Service.Data.DTOs;
using Project.Service.Interfaces;
using Project.Tests;
using Xunit;
using System.Collections.Generic; // Ensure this is included for List<T>

namespace Project.Tests.UnitTests
{
    public class VehicleMakeControllerTests
    {
        private readonly Mock<IVehicleService> _mockService;
        private readonly IMapper _mapper;
        private readonly VehicleMakeController _controller;

        public VehicleMakeControllerTests()
        {
            _mockService = new Mock<IVehicleService>();
            _mapper = TestHelpers.CreateTestMapper(); // Use validated mapper
            
            // Configure mock service
            _mockService.Setup(s => s.GetMakesAsync(1, 10, "Name", "asc", ""))
                .ReturnsAsync(new PagedResult<VehicleMakeDTO>
                {
                    Items = new List<VehicleMakeDTO>
                    {
                        new VehicleMakeDTO { Id = 1, Name = "TestMake", Abbreviation = "TM" }
                    },
                    TotalCount = 1,
                    PageNumber = 1,
                    PageSize = 10
                });

            _controller = new VehicleMakeController(_mockService.Object, _mapper);
        }

        [Fact]
        public async Task Index_ReturnsViewWithMakes()
        {
            // Act
            var result = await _controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<PagedResult<VehicleMakeVM>>(viewResult.Model);
            
            Assert.Single(model.Items);
            Assert.Equal("TestMake", model.Items[0].Name);
        }
    }
}
