using System;
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
using System.Collections.Generic;
using System.Linq;

namespace Project.Tests.IntegrationTests
{
    public class FullRequirementTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public FullRequirementTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory.WithWebHostBuilder(builder =>
            {
                // Corrected environment configuration
                builder.ConfigureAppConfiguration((context, config) =>
                {
                    context.HostingEnvironment.EnvironmentName = "Test";
                });
                
                builder.ConfigureServices(services =>
                {
                    var descriptor = services.SingleOrDefault(
                        d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
                    if (descriptor != null) services.Remove(descriptor);

                    services.AddDbContext<ApplicationDbContext>(options => 
                        options.UseInMemoryDatabase("TestDB"));
                });
            });
        }

        [Fact]
        public async Task CRUD_Workflow_ReturnsProperStatusCodes()
        {
            var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                BaseAddress = new Uri("https://localhost")
            });

            // CREATE
            var csrfToken = await ExtractCsrfToken(client, "/VehicleMake/Create");
            var createResponse = await client.PostAsync("/VehicleMake/Create", new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("__RequestVerificationToken", csrfToken),
                new KeyValuePair<string, string>("Name", "TestMake"),
                new KeyValuePair<string, string>("Abbreviation", "TMK")
            }));
            Assert.Equal(HttpStatusCode.Redirect, createResponse.StatusCode);

            // READ
            var indexResponse = await client.GetAsync("/VehicleMake");
            Assert.Equal(HttpStatusCode.OK, indexResponse.StatusCode);

            // Get ID
            var indexContent = await indexResponse.Content.ReadAsStringAsync();
            var id = ExtractFirstId(indexContent);

            // UPDATE
            var editResponse = await client.GetAsync($"/VehicleMake/Edit/{id}");
            Assert.Equal(HttpStatusCode.OK, editResponse.StatusCode);

            // DELETE
            var deleteResponse = await client.GetAsync($"/VehicleMake/Delete/{id}");
            Assert.Equal(HttpStatusCode.OK, deleteResponse.StatusCode);
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

        private int ExtractFirstId(string html)
        {
            var match = Regex.Match(html, @"data-id=""(\d+)""");
            return match.Success ? int.Parse(match.Groups[1].Value) : -1;
        }
    }
}