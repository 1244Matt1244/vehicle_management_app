using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Project.MVC.Controllers;
using Project.MVC.Helpers;
using Project.MVC.Mappings;
using Project.MVC.ViewModels;
using Project.Service.Data.DTOs;
using Project.Service.Data.Helpers;
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
            var config = new MapperConfiguration(cfg => 
                cfg.AddProfile<MvcMappingProfile>());
            _mapper = config.CreateMapper();

            // Setup for GetMakesAsync
            _mockService.Setup(s => s.GetMakesAsync(
                It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()
            )).ReturnsAsync(
                new PaginatedList<VehicleMakeDTO>(
                    new List<VehicleMakeDTO> { new VehicleMakeDTO { Id = 1, Name = "Test", Abbreviation = "T" } },
                    totalCount: 1,
                    pageIndex: 1,
                    pageSize: 10
                )
            );

            // Setup for valid ID (1)
            _mockService.Setup(s => s.GetMakeByIdAsync(1))
                .ReturnsAsync(new VehicleMakeDTO { Id = 1, Name = "Test", Abbreviation = "T" });

            // Setup for invalid ID (999) - returns a new instance instead of null
            _mockService.Setup(s => s.GetMakeByIdAsync(999))
                .ReturnsAsync(new VehicleMakeDTO()); // Return a default instance instead of null

            _controller = new VehicleMakeController(_mockService.Object, _mapper);
        }

        [Fact]
        public async Task Index_ReturnsViewWithMakes()
        {
            var result = await _controller.Index();
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsType<PagedResult<VehicleMakeVM>>(viewResult.Model);
        }

        [Fact]
        public async Task GetById_ReturnsNotFound_ForInvalidId()
        {
            // Act
            var result = await _controller.Details(999);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
