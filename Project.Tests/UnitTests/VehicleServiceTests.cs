// UnitTests/ServiceTests.cs
using Moq;
using Project.Data.Repositories;
using Project.Service.Services;
using Xunit;

public class VehicleServiceTests
{
    [Fact]
    public async Task GetAllMakesAsync_ReturnsPagedResults()
    {
        // Arrange
        var mockRepo = new Mock<IVehicleMakeRepository>();
        mockRepo.Setup(repo => repo.GetAllMakesPagedAsync(1, 10))
                .ReturnsAsync((new List<VehicleMake>(), 0));

        var service = new VehicleService(mockRepo.Object, AutoMapperConfig.Configure());

        // Act
        var result = await service.GetAllMakesAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(0, result.TotalCount);
    }
}