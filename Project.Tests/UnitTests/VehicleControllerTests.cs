using Xunit;
using Moq;
using Project.MVC.Controllers;
using Project.Service.Interfaces;
using Project.Service.Data.DTOs;
using Project.Service.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;  // For ViewResult
using Project.Service.Mappings;  // For ServiceMappingProfile
using Project.Service.Data.Helpers;    // For PaginatedList<>
using Project.MVC.ViewModels;    // For VehicleMakeVM

public class VehicleControllerTests
{
    private readonly Mock<IVehicleService> _mockService = null!;
    private VehicleMakeController _controller;

    public VehicleControllerTests()
    {
        _mockService = new Mock<IVehicleService>();
        
        var mappedItems = new List<VehicleMakeVM>
        {
            new VehicleMakeVM { Id = 1, Name = "Ford", Abbreviation = "F" },
            new VehicleMakeVM { Id = 2, Name = "BMW", Abbreviation = "B" }
        };
        
        _mockService.Setup(s => s.GetMakesAsync(1, 10, "", "", ""))
            .ReturnsAsync(new PaginatedList<VehicleMakeDTO>(
                new List<VehicleMakeDTO>(), 2, 1, 10));

        _controller = new VehicleMakeController(_mockService.Object, TestHelpers.CreateTestMapper());
    }

    [Fact]
    public async Task Index_ReturnsViewWithMakes()
    {
        // Act
        var result = await _controller.Index("", "", "");

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<PaginatedList<VehicleMakeVM>>(viewResult.Model);
        Assert.NotNull(model);
    }
}
