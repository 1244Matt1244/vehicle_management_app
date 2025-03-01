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

namespace Project.Tests.IntegrationTests
{
    public class VehicleMakeTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public VehicleMakeTests(WebApplicationFactory<Program> factory)
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

                    // Add in-memory database
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

            // Get CSRF token
            var csrfToken = await ExtractCsrfToken(client, "/VehicleMake/Create");
            Assert.NotNull(csrfToken);

            // Create
            var createResponse = await client.PostAsync("/VehicleMake/Create", new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("__RequestVerificationToken", csrfToken),
                new KeyValuePair<string, string>("Name", "TestMake")
            }));
            Assert.Equal(HttpStatusCode.Redirect, createResponse.StatusCode);

            // Read
            var indexResponse = await client.GetAsync("/VehicleMake");
            indexResponse.EnsureSuccessStatusCode();
            var indexContent = await indexResponse.Content.ReadAsStringAsync();
            Assert.Contains("TestMake", indexContent);

            // Update
            var editToken = await ExtractCsrfToken(client, "/VehicleMake/Edit/1");
            var updateResponse = await client.PostAsync("/VehicleMake/Edit/1", new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("__RequestVerificationToken", editToken),
                new KeyValuePair<string, string>("Name", "UpdatedMake")
            }));
            Assert.Equal(HttpStatusCode.Redirect, updateResponse.StatusCode);

            // Delete
            var deleteToken = await ExtractCsrfToken(client, "/VehicleMake/Delete/1");
            var deleteResponse = await client.PostAsync("/VehicleMake/Delete/1", new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("__RequestVerificationToken", deleteToken)
            }));
            Assert.Equal(HttpStatusCode.Redirect, deleteResponse.StatusCode);
        }

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
    }
}