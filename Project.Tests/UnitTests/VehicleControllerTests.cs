using Xunit;
using Moq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Project.MVC.Controllers;
using Project.Service.Interfaces;
using Project.Service.Data.DTOs;
using Project.Service.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Project.Service.Mappings;
using Project.Service.Data.Helpers;
using Project.MVC.ViewModels;
using Project.MVC.Helpers;
using Project.Tests;

namespace Project.Tests.UnitTests
{
    public class VehicleMakeControllerTests
    {
        private readonly Mock<IVehicleService> _serviceMock;
        private readonly IMapper _mapper;
        private readonly VehicleMakeController _controller;

        public VehicleMakeControllerTests()
        {
            _serviceMock = new Mock<IVehicleService>();
            _mapper = TestHelpers.CreateTestMapper();
            _controller = new VehicleMakeController(_serviceMock.Object, _mapper);
        }

        [Fact]
        public async Task Index_ReturnsViewWithMakes()
        {
            // Arrange
            var makes = new PaginatedList<VehicleMakeDTO>(
                new List<VehicleMakeDTO> 
                { 
                    new VehicleMakeDTO { Id = 1, Name = "TestMake", Abbreviation = "TM" } 
                },
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
            var model = Assert.IsAssignableFrom<PagedResult<VehicleMakeVM>>(viewResult.Model);
            Assert.Single(model.Items);
            Assert.Equal(1, model.TotalCount);
        }
    }
}