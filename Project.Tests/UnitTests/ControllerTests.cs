using Xunit;
using Moq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Project.MVC.Controllers;
using Project.Service.Interfaces;
using Project.Service.Data.DTOs;
using Project.Service.Data.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Project.Tests.UnitTests.ControllerTests
{
    public class VehicleMakeControllerTests
    {
        private readonly Mock<IVehicleService> _serviceMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly VehicleMakeController _controller;

        public VehicleMakeControllerTests()
        {
            _serviceMock = new Mock<IVehicleService>();
            _mapperMock = new Mock<IMapper>();
            _controller = new VehicleMakeController(_serviceMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Index_ReturnsViewWithMakes()
        {
            // Arrange
            var makes = new PaginatedList<VehicleMakeDTO>(
                new List<VehicleMakeDTO> { new VehicleMakeDTO { Name = "Test", Abrv = "T" } },
                totalCount: 1,
                pageIndex: 1,
                pageSize: 10
            );

            _serviceMock.Setup(s => s.GetMakesAsync(1, 10, "", "", ""))
                    .ReturnsAsync(makes);

            // Act
            var result = await _controller.Index(1, 10, "", "", "");

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(makes, viewResult.Model);
        }
    }
}