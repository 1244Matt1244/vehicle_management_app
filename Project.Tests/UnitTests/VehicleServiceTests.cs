// Project.UnitTests/ServiceTests.cs
using Moq;
using Project.Service.Repositories;
using Project.Service.Services;
using Xunit;

public class VehicleServiceTests
{
    [Fact]
    public async Task GetMakesAsync_ReturnsPaginatedList()
    {
        // Arrange
        var mockRepo = new Mock<IVehicleRepository>();
        mockRepo.Setup(repo => repo.GetMakesPaginatedAsync(1, 10, "Name", "asc", ""))
                .ReturnsAsync((new List<VehicleMake>(), 0));

        var service = new VehicleService(mockRepo.Object, AutoMapperConfig.Configure());

        // Act
        var result = await service.GetMakesAsync(1, 10, "Name", "asc", "");

        // Assert
        Assert.NotNull(result);
        Assert.Equal(0, result.TotalCount);
    }
}