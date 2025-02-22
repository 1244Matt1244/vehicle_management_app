using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Project.MVC.Controllers;
using Project.Service.Interfaces;
using Project.Service.Data.DTOs;
using Xunit;

namespace Project.Tests.UnitTests.ControllerTests
{
    public class MakeControllerTests
    {
        private readonly Mock<IVehicleService> _serviceMock;
        private readonly Mock<IMapper> _mapperMock;

        public MakeControllerTests()
        {
            _serviceMock = new Mock<IVehicleService>();
            _mapperMock = new Mock<IMapper>();
        }

        [Fact]
        public async Task Index_ReturnsViewResult()
        {
            var makes = new PaginatedList<VehicleMakeDTO>(new List<VehicleMakeDTO> { new VehicleMakeDTO { Name = "TestMake", Abrv = "TM" } }, 1, 10, 1);
            _serviceMock.Setup(s => s.GetMakesAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(makes);

            var controller = new VehicleMakeController(_serviceMock.Object);

            var result = await controller.Index(1, 10, "", "", "") as ViewResult;

            Assert.NotNull(result);
        }
    }
}
