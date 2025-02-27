using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using Project.MVC;

namespace Project.Tests.IntegrationTests
{
    public class FullRequirementTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public FullRequirementTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Get_VehicleMakes_ReturnsSuccess()
        {
            var response = await _client.GetAsync("/vehiclemake");
            response.EnsureSuccessStatusCode();
        }
    }
}