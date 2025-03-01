using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit;
using Project.MVC;

namespace Project.Tests.IntegrationTests
{
    public class FullRequirementTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public FullRequirementTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    // Explicitly add options and configure RouteOptions
                    services.AddOptions<RouteOptions>()
                        .Configure(options => options.LowercaseUrls = true);
                });
            }).CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = true,
                HandleCookies = true
            });
        }

        [Fact]
        public async Task CRUD_Workflow_ReturnsProperStatusCodes()
        {
            // Create - First ensure CSRF token is fetched
            var csrfToken = await ExtractCsrfToken("/vehiclemake/create");
            var createContent = new FormUrlEncodedContent(new[] 
            {
                new KeyValuePair<string, string>("Name", "TestMake"),
                new KeyValuePair<string, string>("Abbreviation", "TMK"),
                new KeyValuePair<string, string>("__RequestVerificationToken", csrfToken)
            });
            var createResponse = await _client.PostAsync("/vehiclemake/create", createContent);
            Assert.Equal(HttpStatusCode.OK, createResponse.StatusCode);

            // Extract ID from the index page
            var indexHtml = await _client.GetStringAsync("/vehiclemake");
            var idMatch = Regex.Match(indexHtml, @"data-id=""(\d+)""");
            Assert.True(idMatch.Success, "No ID found in the index page.");
            var id = idMatch.Groups[1].Value;

            // Update - Ensure token is valid for editing
            var editToken = await ExtractCsrfToken($"/vehiclemake/edit/{id}");
            var updateContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("Name", "UpdatedMake"),
                new KeyValuePair<string, string>("Abbreviation", "UMK"),
                new KeyValuePair<string, string>("__RequestVerificationToken", editToken)
            });
            var updateResponse = await _client.PostAsync($"/vehiclemake/edit/{id}", updateContent);
            Assert.Equal(HttpStatusCode.OK, updateResponse.StatusCode);

            // Delete - Ensure token is valid for deleting
            var deleteToken = await ExtractCsrfToken($"/vehiclemake/delete/{id}");
            var deleteContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("__RequestVerificationToken", deleteToken)
            });
            var deleteResponse = await _client.PostAsync($"/vehiclemake/delete/{id}", deleteContent);
            Assert.Equal(HttpStatusCode.OK, deleteResponse.StatusCode);
        }

        private async Task<string> ExtractCsrfToken(string url)
        {
            var response = await _client.GetAsync(url);
            response.EnsureSuccessStatusCode(); // Ensure the response is successful
            var html = await response.Content.ReadAsStringAsync();
            
            // Regex to find the CSRF token
            var match = Regex.Match(html, 
                @"<input[^>]*name=[""']__RequestVerificationToken[""'][^>]*value=[""']([^""']+)[""']",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);
            
            if (match.Success)
            {
                return match.Groups[1].Value;
            }
            else
            {
                throw new Exception($"CSRF token not found at {url}\nHTML:{html}");
            }
        }
    }
}
