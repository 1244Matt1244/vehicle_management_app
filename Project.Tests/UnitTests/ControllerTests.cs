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
[TestClass]
public class VehicleMakeControllerTests
{
    private VehicleMakeController _controller;
    private IMapper _mapper;

    [TestInitialize]
    public void Setup()
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

    [TestMethod]
    public async Task Index_ReturnsViewWithMakes()
    {
        // Act
        var result = await _controller.Index();

        // Assert
        Assert.IsInstanceOfType(result, typeof(ViewResult));
    }
    
}
}
