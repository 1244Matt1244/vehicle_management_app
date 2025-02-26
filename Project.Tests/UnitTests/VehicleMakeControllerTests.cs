using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Project.MVC.Controllers;
using Project.MVC.Helpers;
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
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MapperConfig>());
            _mapper = config.CreateMapper();

            // Mock service to return PaginatedList<VehicleMakeDTO>
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

            _controller = new VehicleMakeController(_mockService.Object, _mapper);
        }

        [Fact]
        public async Task Index_ReturnsViewWithMakes()
        {
            var result = await _controller.Index();
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsType<PaginatedList<VehicleMakeVM>>(viewResult.Model);
        }
    }
}