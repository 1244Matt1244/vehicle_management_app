// Project.Tests/Integration/FullRequirementTests.cs

using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Project.Tests.Integration
{
    public class FullSystemTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public FullSystemTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task FullCRUDWorkflow_ReturnsProperStatusCodes()
        {
            // Create (POST)
            var createResponse = await _client.PostAsJsonAsync("/VehicleMake", new 
            {
                Name = "TestMake",
                Abbreviation = "TM"
            });
            Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);

            // Read (GET)
            var getResponse = await _client.GetAsync(createResponse.Headers.Location);
            Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);

            // Update (PUT)
            var updateResponse = await _client.PutAsJsonAsync($"/VehicleMake/1", new 
            {
                Id = 1,
                Name = "UpdatedMake",
                Abbreviation = "UM"
            });
            Assert.Equal(HttpStatusCode.NoContent, updateResponse.StatusCode);

            // Delete (DELETE)
            var deleteResponse = await _client.DeleteAsync("/VehicleMake/1");
            Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);

            // Verify deletion
            var verifyDelete = await _client.GetAsync("/VehicleMake/1");
            Assert.Equal(HttpStatusCode.NotFound, verifyDelete.StatusCode);
        }
    }
}
