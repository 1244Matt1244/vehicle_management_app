using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Project.MVC.Controllers;
using Project.MVC.Helpers;
using Project.MVC.ViewModels;
using Project.Service.Data.DTOs;
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
            
            // Configure AutoMapper mock
            var config = new MapperConfiguration(cfg => 
                cfg.CreateMap<VehicleMakeDTO, VehicleMakeVM>());
            _mapper = config.CreateMapper();

            // Correct mock setup with exact parameter matching
            _mockService.Setup(s => s.GetMakesAsync(
                1,                      // pageNumber
                10,                     // pageSize
                It.IsAny<string>(),     // sortBy
                It.IsAny<string>(),     // sortOrder
                It.IsAny<string>()      // searchString
            )).ReturnsAsync(new PagedResult<VehicleMakeDTO>
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
            Assert.Equal("TestMake", model.Items[0].Name);
        }
    }
}