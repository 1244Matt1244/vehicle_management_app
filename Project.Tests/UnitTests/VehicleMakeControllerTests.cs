using Xunit;
using Moq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Project.MVC.Controllers;
using Project.Service.Interfaces;
using Project.Service.Data.DTOs;
using Project.Service.Models;
using Project.Tests.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;
using Project.Service.Mappings;  // For ServiceMappingProfile
using Project.Service.Data.Helpers;    // For PaginatedList<>
using Project.MVC.ViewModels;  

public class VehicleMakeControllerTests
{
    private readonly Mock<IVehicleService> _serviceMock = null!;
    private readonly IMapper _mapper;
    private readonly VehicleMakeController _controller;

    public VehicleMakeControllerTests()
    {
        _serviceMock = new Mock<IVehicleService>();

        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<ServiceMappingProfile>();
            cfg.CreateMap<PaginatedList<VehicleMake>, PaginatedList<VehicleMakeDTO>>()
                .ConvertUsing<PaginatedListConverter<VehicleMake, VehicleMakeDTO>>();
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
        var model = Assert.IsAssignableFrom<PaginatedList<VehicleMakeDTO>>(viewResult.Model);
        Assert.Equal(makes.Items.Count, model.Items.Count);
        Assert.Equal(makes.TotalCount, model.TotalCount);
        Assert.Equal(makes.PageIndex, model.PageIndex);
        Assert.Equal(makes.PageSize, model.PageSize);
    }
}
