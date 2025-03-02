using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Project.MVC;
using Project.Service.Data.Context;
using System.Linq;
using System.Collections.Generic;

namespace Project.Tests.IntegrationTests
{
    public class FullRequirementTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;

        public FullRequirementTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    // Remove existing database configuration
                    var descriptor = services.SingleOrDefault(
                        d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
                    if (descriptor != null)
                    {
                        services.Remove(descriptor);
                    }

                    // Add in-memory database for testing
                    services.AddDbContext<ApplicationDbContext>(options =>
                    {
                        options.UseInMemoryDatabase("TestDatabase");
                    });
                });
            });

            _client = _factory.CreateClient();
        }

        [Fact]
        public async Task Full_CRUD_Workflow_With_CSRF()
        {
            // Create a new VehicleMake - Get CSRF token
            var csrfToken = await ExtractCsrfToken("/VehicleMake/Create");
            Assert.NotNull(csrfToken);

            // Create new VehicleMake
            var createResponse = await _client.PostAsync("/VehicleMake/Create", new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("__RequestVerificationToken", csrfToken),
                new KeyValuePair<string, string>("Name", "TestMake"),
                new KeyValuePair<string, string>("Abbreviation", "TMK")
            }));
            Assert.Equal(HttpStatusCode.Redirect, createResponse.StatusCode);

            // Read - Get the index page and verify the newly created VehicleMake
            var indexResponse = await _client.GetAsync("/VehicleMake");
            indexResponse.EnsureSuccessStatusCode();
            var indexContent = await indexResponse.Content.ReadAsStringAsync();
            Assert.Contains("TestMake", indexContent);

            // Extract ID with robust regex
            var idMatch = Regex.Match(indexContent, @"<tr[^>]*data-id=""(\d+)""[^>]*>");
            Assert.True(idMatch.Success, "No ID found in the index page.");
            var id = idMatch.Groups[1].Value;

            // Update - Get CSRF token for editing the newly created VehicleMake
            var editToken = await ExtractCsrfToken($"/VehicleMake/Edit/{id}");
            var updateResponse = await _client.PostAsync($"/VehicleMake/Edit/{id}", new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("__RequestVerificationToken", editToken),
                new KeyValuePair<string, string>("Name", "UpdatedMake"),
                new KeyValuePair<string, string>("Abbreviation", "UMK")
            }));
            Assert.Equal(HttpStatusCode.Redirect, updateResponse.StatusCode);

            // Read - Verify the updated VehicleMake on the index page
            var updatedIndexResponse = await _client.GetAsync("/VehicleMake");
            updatedIndexResponse.EnsureSuccessStatusCode();
            var updatedIndexContent = await updatedIndexResponse.Content.ReadAsStringAsync();
            Assert.Contains("UpdatedMake", updatedIndexContent);

            // Delete - Get CSRF token for deleting the VehicleMake
            var deleteToken = await ExtractCsrfToken($"/VehicleMake/Delete/{id}");
            var deleteResponse = await _client.PostAsync($"/VehicleMake/Delete/{id}", new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("__RequestVerificationToken", deleteToken)
            }));
            Assert.Equal(HttpStatusCode.Redirect, deleteResponse.StatusCode);

            // Read - Verify the VehicleMake was deleted
            var deletedIndexResponse = await _client.GetAsync("/VehicleMake");
            deletedIndexResponse.EnsureSuccessStatusCode();
            var deletedIndexContent = await deletedIndexResponse.Content.ReadAsStringAsync();
            Assert.DoesNotContain("UpdatedMake", deletedIndexContent);
        }

        // Helper method to extract CSRF token from the HTML page
        private async Task<string> ExtractCsrfToken(string url)
        {
            var response = await _client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var html = await response.Content.ReadAsStringAsync();

            var match = Regex.Match(html, 
                @"<input[^>]*name=[""']__RequestVerificationToken[""'][^>]*value=[""']([^""']+)[""']",
                RegexOptions.IgnoreCase);

            return match.Success ? match.Groups[1].Value : null!;
        }

        [Fact]
        public async Task CRUD_Workflow_ReturnsProperStatusCodes()
        {
            // Test the index page
            var indexResponse = await _client.GetAsync("/VehicleMake");
            Assert.Equal(HttpStatusCode.OK, indexResponse.StatusCode);

            // Test create page
            var createResponse = await _client.GetAsync("/VehicleMake/Create");
            Assert.Equal(HttpStatusCode.OK, createResponse.StatusCode);

            // Create a new VehicleMake and check status
            var csrfToken = await ExtractCsrfToken("/VehicleMake/Create");
            var postCreateResponse = await _client.PostAsync("/VehicleMake/Create", new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("Name", "TestMake"),
                new KeyValuePair<string, string>("Abbreviation", "TMK"),
                new KeyValuePair<string, string>("__RequestVerificationToken", csrfToken)
            }));
            Assert.Equal(HttpStatusCode.Redirect, postCreateResponse.StatusCode);

            // Extract ID for edit and delete tests
            var indexHtml = await _client.GetStringAsync("/VehicleMake");
            var idMatch = Regex.Match(indexHtml, @"<tr[^>]*data-id=""(\d+)""[^>]*>");
            Assert.True(idMatch.Success, "No ID found in the index page.");
            var id = idMatch.Groups[1].Value;

            // Test edit page for a vehicle make
            var editResponse = await _client.GetAsync($"/VehicleMake/Edit/{id}");
            Assert.Equal(HttpStatusCode.OK, editResponse.StatusCode);

            // Test delete page for a vehicle make
            var deleteResponse = await _client.GetAsync($"/VehicleMake/Delete/{id}");
            Assert.Equal(HttpStatusCode.OK, deleteResponse.StatusCode);
        }
    }
}
