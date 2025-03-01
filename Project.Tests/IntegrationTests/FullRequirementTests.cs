using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Project.MVC;
using Project.Service.Data;
using System.Linq;
using System.Collections.Generic;

namespace Project.Tests.IntegrationTests
{
    public class FullRequirementTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

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
        }

        [Fact]
        public async Task Full_CRUD_Workflow_With_CSRF()
        {
            var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

            // Create a new VehicleMake - Get CSRF token
            var csrfToken = await ExtractCsrfToken(client, "/VehicleMake/Create");
            Assert.NotNull(csrfToken);

            // Create new VehicleMake
            var createResponse = await client.PostAsync("/VehicleMake/Create", new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("__RequestVerificationToken", csrfToken),
                new KeyValuePair<string, string>("Name", "TestMake")
            }));
            Assert.Equal(HttpStatusCode.Redirect, createResponse.StatusCode);

            // Read - Get the index page and verify the newly created VehicleMake
            var indexResponse = await client.GetAsync("/VehicleMake");
            indexResponse.EnsureSuccessStatusCode();
            var indexContent = await indexResponse.Content.ReadAsStringAsync();
            Assert.Contains("TestMake", indexContent);

            // Update - Get CSRF token for editing the newly created VehicleMake
            var editToken = await ExtractCsrfToken(client, "/VehicleMake/Edit/1");
            var updateResponse = await client.PostAsync("/VehicleMake/Edit/1", new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("__RequestVerificationToken", editToken),
                new KeyValuePair<string, string>("Name", "UpdatedMake")
            }));
            Assert.Equal(HttpStatusCode.Redirect, updateResponse.StatusCode);

            // Read - Verify the updated VehicleMake on the index page
            var updatedIndexResponse = await client.GetAsync("/VehicleMake");
            updatedIndexResponse.EnsureSuccessStatusCode();
            var updatedIndexContent = await updatedIndexResponse.Content.ReadAsStringAsync();
            Assert.Contains("UpdatedMake", updatedIndexContent);

            // Delete - Get CSRF token for deleting the VehicleMake
            var deleteToken = await ExtractCsrfToken(client, "/VehicleMake/Delete/1");
            var deleteResponse = await client.PostAsync("/VehicleMake/Delete/1", new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("__RequestVerificationToken", deleteToken)
            }));
            Assert.Equal(HttpStatusCode.Redirect, deleteResponse.StatusCode);

            // Read - Verify the VehicleMake was deleted
            var deletedIndexResponse = await client.GetAsync("/VehicleMake");
            deletedIndexResponse.EnsureSuccessStatusCode();
            var deletedIndexContent = await deletedIndexResponse.Content.ReadAsStringAsync();
            Assert.DoesNotContain("UpdatedMake", deletedIndexContent);
        }

        // Helper method to extract CSRF token from the HTML page
        private async Task<string> ExtractCsrfToken(HttpClient client, string url)
        {
            var response = await client.GetAsync(url);
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
            var client = _factory.CreateClient();

            // Test the index page
            var indexResponse = await client.GetAsync("/VehicleMake");
            Assert.Equal(HttpStatusCode.OK, indexResponse.StatusCode);

            // Test create page
            var createResponse = await client.GetAsync("/VehicleMake/Create");
            Assert.Equal(HttpStatusCode.OK, createResponse.StatusCode);

            // Test edit page for a vehicle make
            var editResponse = await client.GetAsync("/VehicleMake/Edit/1");
            Assert.Equal(HttpStatusCode.OK, editResponse.StatusCode);

            // Test delete page for a vehicle make
            var deleteResponse = await client.GetAsync("/VehicleMake/Delete/1");
            Assert.Equal(HttpStatusCode.OK, deleteResponse.StatusCode);
        }
    }
}
