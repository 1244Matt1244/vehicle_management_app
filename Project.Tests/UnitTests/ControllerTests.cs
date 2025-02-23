using Xunit;
using Moq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Project.MVC.Controllers;
using Project.Service.Models;
using Project.Service.Interfaces;
using Project.Service.Data.DTOs;
using Project.Service.Data.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;
using Project.Service.Mappings;

namespace Project.Tests.UnitTests.ControllerTests
{
    public class VehicleMakeControllerTests
    {
        private readonly Mock<IVehicleService> _serviceMock;
        private readonly IMapper _mapper;
        private readonly VehicleMakeController _controller;

        public VehicleMakeControllerTests()
        {
            _serviceMock = new Mock<IVehicleService>();

            // Update the MapperConfiguration to ensure that PaginatedList mappings are included
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ServiceMappingProfile>(); // Ensure this profile is added for correct mapping
                cfg.CreateMap<PaginatedList<VehicleMake>, PaginatedList<VehicleMakeDTO>>()
                    .ConvertUsing<PaginatedListConverter<VehicleMake, VehicleMakeDTO>>(); // Register the converter
            });

            _mapper = config.CreateMapper();
            _controller = new VehicleMakeController(_serviceMock.Object, _mapper);
        }

        [Fact]
        public async Task Index_ReturnsViewWithMakes()
        {
            // Arrange
            var makes = new PaginatedList<VehicleMakeDTO>(
                new List<VehicleMakeDTO>
                { 
                    new VehicleMakeDTO { Id = 1, Name = "TestMake", Abrv = "TM" } 
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
            var model = Assert.IsAssignableFrom<PaginatedList<VehicleMakeDTO>>(viewResult.Model); // Ensure the model is the correct type
            Assert.Equal(makes.Items.Count, model.Items.Count); // Ensure the number of items matches
            Assert.Equal(makes.TotalCount, model.TotalCount); // Ensure the total count matches
            Assert.Equal(makes.PageIndex, model.PageIndex); // Ensure page index matches
            Assert.Equal(makes.PageSize, model.PageSize); // Ensure page size matches
        }
    }
}
