using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Project.MVC.Controllers;
using Project.MVC.Helpers;
using Project.MVC.ViewModels;
using Project.Service.Data.DTOs;
using Project.Service.Data.Helpers; // Add this namespace
using Project.Service.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

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
            
            // Configure AutoMapper
            var config = new MapperConfiguration(cfg => 
            {
                cfg.CreateMap<VehicleMakeDTO, VehicleMakeVM>();
            });
            _mapper = config.CreateMapper();

            // Mock service to return PaginatedList<VehicleMakeDTO>
            _mockService.Setup(s => s.GetMakesAsync(
                It.IsAny<int>(),      // pageIndex
                It.IsAny<int>(),      // pageSize
                It.IsAny<string>(),   // sortBy
                It.IsAny<string>(),   // sortOrder
                It.IsAny<string>()    // searchString
            )).ReturnsAsync(
                new PaginatedList<VehicleMakeDTO>(
                    items: new List<VehicleMakeDTO> 
                    { 
                        new VehicleMakeDTO { Id = 1, Name = "TestMake", Abbreviation = "TM" } 
                    },
                    totalCount: 1,
                    pageIndex: 1,     // Match parameter name from PaginatedList constructor
                    pageSize: 10
                )
            );

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