using System.Net;
using System.Net.Http;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using Project.MVC;

namespace Project.Tests.IntegrationTests
{
    public class FullCRUDTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public FullCRUDTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task CRUD_Workflow_ReturnsProperStatusCodes()
        {
            // Create: POST to create a new VehicleMake.
            var postResponse = await _client.PostAsync("/VehicleMake", new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("Name", "TestMake"),
                new KeyValuePair<string, string>("Abbreviation", "TM")
            }));
            Assert.Equal(HttpStatusCode.OK, postResponse.StatusCode);

            // Read: GET the list of VehicleMakes.
            var getResponse = await _client.GetAsync("/VehicleMake");
            getResponse.EnsureSuccessStatusCode();
            var getContent = await getResponse.Content.ReadAsStringAsync();

            // Extract the created VehicleMake id from the HTML response.
            // This regex searches for an anchor tag with href="/VehicleMake/Edit/{id}".
            var match = Regex.Match(getContent, @"href\s*=\s*[""']\/VehicleMake\/Edit\/(\d+)[""']");
            Assert.True(match.Success, "No VehicleMake id found in the response.");
            var id = match.Groups[1].Value;

            // Update: PUT to update the VehicleMake using the extracted id.
            var putResponse = await _client.PutAsync($"/VehicleMake/{id}", new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("Name", "UpdatedMake"),
                new KeyValuePair<string, string>("Abbreviation", "UM")
            }));
            Assert.Equal(HttpStatusCode.OK, putResponse.StatusCode);

            // Delete: DELETE the VehicleMake using the extracted id.
            var deleteResponse = await _client.DeleteAsync($"/VehicleMake/{id}");
            Assert.Equal(HttpStatusCode.OK, deleteResponse.StatusCode);
        }
    }
}
