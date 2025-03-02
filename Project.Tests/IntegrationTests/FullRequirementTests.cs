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
                    var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
                    if (descriptor != null)
                    {
                        services.Remove(descriptor);
                    }

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
            var csrfToken = await ExtractCsrfToken("/VehicleMake/Create");
            Assert.NotNull(csrfToken);

            var createResponse = await _client.PostAsync("/VehicleMake/Create", new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("__RequestVerificationToken", csrfToken),
                new KeyValuePair<string, string>("Name", "TestMake"),
                new KeyValuePair<string, string>("Abbreviation", "TMK")
            }));
            Assert.Equal(HttpStatusCode.Redirect, createResponse.StatusCode);

            var indexResponse = await _client.GetAsync("/VehicleMake");
            indexResponse.EnsureSuccessStatusCode();
            var indexContent = await indexResponse.Content.ReadAsStringAsync();
            Assert.Contains("TestMake", indexContent);

            var idMatch = Regex.Match(indexContent, @"<tr[^>]*data-id=""(\d+)""[^>]*>");
            Assert.True(idMatch.Success, "No ID found in the index page.");
            var id = idMatch.Groups[1].Value;

            var editToken = await ExtractCsrfToken($"/VehicleMake/Edit/{id}");
            var updateResponse = await _client.PostAsync($"/VehicleMake/Edit/{id}", new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("__RequestVerificationToken", editToken),
                new KeyValuePair<string, string>("Name", "UpdatedMake"),
                new KeyValuePair<string, string>("Abbreviation", "UMK")
            }));
            Assert.Equal(HttpStatusCode.Redirect, updateResponse.StatusCode);

            var updatedIndexResponse = await _client.GetAsync("/VehicleMake");
            updatedIndexResponse.EnsureSuccessStatusCode();
            var updatedIndexContent = await updatedIndexResponse.Content.ReadAsStringAsync();
            Assert.Contains("UpdatedMake", updatedIndexContent);

            var deleteToken = await ExtractCsrfToken($"/VehicleMake/Delete/{id}");
            var deleteResponse = await _client.PostAsync($"/VehicleMake/Delete/{id}", new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("__RequestVerificationToken", deleteToken)
            }));
            Assert.Equal(HttpStatusCode.Redirect, deleteResponse.StatusCode);

            var deletedIndexResponse = await _client.GetAsync("/VehicleMake");
            deletedIndexResponse.EnsureSuccessStatusCode();
            var deletedIndexContent = await deletedIndexResponse.Content.ReadAsStringAsync();
            Assert.DoesNotContain("UpdatedMake", deletedIndexContent);
        }

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
            var indexResponse = await _client.GetAsync("/VehicleMake");
            Assert.Equal(HttpStatusCode.OK, indexResponse.StatusCode);

            var createResponse = await _client.GetAsync("/VehicleMake/Create");
            Assert.Equal(HttpStatusCode.OK, createResponse.StatusCode);

            var csrfToken = await ExtractCsrfToken("/VehicleMake/Create");
            var postCreateResponse = await _client.PostAsync("/VehicleMake/Create", new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("Name", "TestMake"),
                new KeyValuePair<string, string>("Abbreviation", "TMK"),
                new KeyValuePair<string, string>("__RequestVerificationToken", csrfToken)
            }));
            Assert.Equal(HttpStatusCode.Redirect, postCreateResponse.StatusCode);

            var indexHtml = await _client.GetStringAsync("/VehicleMake");
            var idMatch = Regex.Match(indexHtml, @"<tr[^>]*data-id=""(\d+)""[^>]*>");
            Assert.True(idMatch.Success, "No ID found in the index page.");
            var id = idMatch.Groups[1].Value;

            var editResponse = await _client.GetAsync($"/VehicleMake/Edit/{id}");
            Assert.Equal(HttpStatusCode.OK, editResponse.StatusCode);

            var deleteResponse = await _client.GetAsync($"/VehicleMake/Delete/{id}");
            Assert.Equal(HttpStatusCode.OK, deleteResponse.StatusCode);
        }
    }
}
