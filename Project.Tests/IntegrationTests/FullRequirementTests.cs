using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using Project.MVC;
using System.Collections.Generic;

namespace Project.Tests.IntegrationTests
{
    public class FullRequirementTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly WebApplicationFactory<Program> _factory;

        public FullRequirementTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory.WithWebHostBuilder(builder =>
                builder.UseSetting("ConnectionStrings:DefaultConnection", "Your_Test_Connection_String"));
            _client = _factory.CreateClient();
        }

        [Fact]
        public async Task CRUD_Workflow_ReturnsProperStatusCodes()
        {
            // Get CSRF token from create form
            var getCreateResponse = await _client.GetAsync("/VehicleMake/Create");
            getCreateResponse.EnsureSuccessStatusCode();
            var createHtml = await getCreateResponse.Content.ReadAsStringAsync();
            var csrfToken = await ExtractAntiForgeryToken(createHtml);
            Assert.NotNull(csrfToken);

            // Create: POST new VehicleMake
            var postResponse = await _client.SendAsync(CreatePostRequest(
                "/VehicleMake/Create",
                new Dictionary<string, string>
                {
                    {"Name", "TestMake"},
                    {"Abbreviation", "TMK"},
                    {"__RequestVerificationToken", csrfToken!}
                }
            ));
            Assert.Equal(HttpStatusCode.Redirect, postResponse.StatusCode);

            // Follow redirect to Index
            var indexResponse = await _client.GetAsync(postResponse.Headers.Location);
            indexResponse.EnsureSuccessStatusCode();
            var indexContent = await indexResponse.Content.ReadAsStringAsync();

            // Extract ID from table
            var id = ExtractFirstIdFromTable(indexContent);
            Assert.NotNull(id);

            // Get CSRF token for edit form
            var getEditResponse = await _client.GetAsync($"/VehicleMake/Edit/{id}");
            getEditResponse.EnsureSuccessStatusCode();
            var editHtml = await getEditResponse.Content.ReadAsStringAsync();
            var editCsrfToken = await ExtractAntiForgeryToken(editHtml);
            Assert.NotNull(editCsrfToken);

            // Update: PUT changes
            var putResponse = await _client.SendAsync(CreatePostRequest(
                $"/VehicleMake/Edit/{id}",
                new Dictionary<string, string>
                {
                    {"Id", id!},
                    {"Name", "UpdatedMake"},
                    {"Abbreviation", "UMK"},
                    {"__RequestVerificationToken", editCsrfToken!}
                }
            ));
            Assert.Equal(HttpStatusCode.Redirect, putResponse.StatusCode);

            // Get CSRF token for delete form
            var getDeleteResponse = await _client.GetAsync($"/VehicleMake/Delete/{id}");
            getDeleteResponse.EnsureSuccessStatusCode();
            var deleteHtml = await getDeleteResponse.Content.ReadAsStringAsync();
            var deleteCsrfToken = await ExtractAntiForgeryToken(deleteHtml);
            Assert.NotNull(deleteCsrfToken);

            // Delete: POST delete
            var deleteResponse = await _client.SendAsync(CreatePostRequest(
                $"/VehicleMake/Delete/{id}",
                new Dictionary<string, string>
                {
                    {"Id", id!},
                    {"__RequestVerificationToken", deleteCsrfToken!}
                }
            ));
            Assert.Equal(HttpStatusCode.Redirect, deleteResponse.StatusCode);
        }

        private HttpRequestMessage CreatePostRequest(string url, Dictionary<string, string> formData)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Content = new FormUrlEncodedContent(formData);
            return request;
        }

        // Updated regex: more flexible to match input element with anti-forgery token.
        private Task<string?> ExtractAntiForgeryToken(string htmlContent)
        {
            var match = Regex.Match(htmlContent,
                @"<input[^>]+name\s*=\s*[""']__RequestVerificationToken[""'][^>]+value\s*=\s*[""']([^""']+)[""']",
                RegexOptions.IgnoreCase);
            return Task.FromResult(match.Success ? match.Groups[1].Value : null);
        }

        private string? ExtractFirstIdFromTable(string htmlContent)
        {
            var match = Regex.Match(htmlContent, @"<tr>\s*<td[^>]*>(\d+)</td>");
            return match.Success ? match.Groups[1].Value : null;
        }
    }
}
