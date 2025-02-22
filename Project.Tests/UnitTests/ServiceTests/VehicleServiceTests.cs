using Xunit;
using Moq;
using AutoMapper;
using Project.Service.Services;
using Project.Service.Interfaces;
using Project.Service.Data.DTOs;
using Project.Service.Data.Helpers;
using Project.Service.Mappings;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Project.Tests.UnitTests.ServiceTests
{
    public class VehicleServiceTests
    {
        private readonly Mock<IVehicleRepository> _repositoryMock;
        private readonly IMapper _mapper;

        public VehicleServiceTests()
        {
            _repositoryMock = new Mock<IVehicleRepository>();
            var config = new MapperConfiguration(cfg => cfg.AddProfile<ServiceMappingProfile>());
            _mapper = config.CreateMapper();
        }

        [Fact]
        public async Task GetMakesAsync_ReturnsPaginatedList()
        {
            // Arrange
            var makes = new List<VehicleMake>
            {
                new VehicleMake { Id = 1, Name = "BMW", Abrv = "B" },
                new VehicleMake { Id = 2, Name = "Ford", Abrv = "F" }
            };
            var paginatedMakes = new PaginatedList<VehicleMake>(makes, 2, 1, 10);

            _repositoryMock.Setup(r => r.GetMakesPaginatedAsync(1, 10, "Name", "asc", ""))
                .ReturnsAsync(paginatedMakes);

            var service = new VehicleService(_repositoryMock.Object, _mapper);

            // Act
            var result = await service.GetMakesAsync(1, 10, "Name", "asc", "");

            // Assert
            Assert.Equal(2, result.Items.Count);
            Assert.Equal("BMW", result.Items[0].Name);
        }
    }
}