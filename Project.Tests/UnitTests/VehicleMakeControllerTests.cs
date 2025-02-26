using Microsoft.AspNetCore.Mvc;
using Moq;
using Project.MVC.Controllers;
using Project.MVC.ViewModels;
using Project.Service.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Project.Tests.UnitTests
{
    public class VehicleMakeControllerTests // Single class definition
    {
        private readonly Mock<IVehicleService> _mockService;
        private readonly VehicleMakeController _controller;

        // Single constructor
        public VehicleMakeControllerTests()
        {
            _mockService = new Mock<IVehicleService>();
            _controller = new VehicleMakeController(
                _mockService.Object, 
                Mock.Of<IMapper>() // Mock AutoMapper
            );
        }

        [Fact]
        public async Task Index_ReturnsViewWithMakes()
        {
            // Arrange
            var mockMakes = new PaginatedList<VehicleMakeVM>(
                new List<VehicleMakeVM> { new VehicleMakeVM() }, 
                totalCount: 1, 
                pageIndex: 1, 
                pageSize: 10
            );

            _mockService.Setup(s => s.GetMakesAsync(
                It.IsAny<int>(), 
                It.IsAny<int>(), 
                It.IsAny<string>(), 
                It.IsAny<string>(), 
                It.IsAny<string>()
            )).ReturnsAsync(mockMakes);

            // Act
            var result = await _controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsAssignableFrom<PagedResult<VehicleMakeVM>>(viewResult.Model);
        }
    }
}