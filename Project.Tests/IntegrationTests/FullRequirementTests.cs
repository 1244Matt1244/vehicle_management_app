using System;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Project.Tests.IntegrationTests
{
    public class FullRequirementTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public FullRequirementTests()
        {
            _factory = new WebApplicationFactory<Program>();
        }

        [Fact]
        public async Task CRUD_Workflow_ReturnsProperStatusCodes()
        {
            var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                BaseAddress = new Uri("https://localhost")
            });

            // Test index page
            var indexResponse = await client.GetAsync("/VehicleMake");
            Assert.Equal(HttpStatusCode.OK, indexResponse.StatusCode);

            // Extract CSRF token and cookie for subsequent requests
            var (csrfToken, cookie) = await ExtractCsrfData(client, "/VehicleMake/Create");
            client.DefaultRequestHeaders.Add("Cookie", cookie);

            // Create
            var createResponse = await client.PostAsync("/VehicleMake/Create", new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("__RequestVerificationToken", csrfToken),
                new KeyValuePair<string, string>("Name", "TestMake"),
                new KeyValuePair<string, string>("Abbreviation", "TMK")
            }));
            Assert.Equal(HttpStatusCode.Redirect, createResponse.StatusCode);

            // Verify creation
            var createdContent = await client.GetStringAsync("/VehicleMake");
            var id = ExtractFirstId(createdContent);
            Assert.True(id > 0, "No valid ID found after creation.");

            // Test edit page
            var editResponse = await client.GetAsync($"/VehicleMake/Edit/{id}");
            Assert.Equal(HttpStatusCode.OK, editResponse.StatusCode);

            // Test delete page
            var deleteResponse = await client.GetAsync($"/VehicleMake/Delete/{id}");
            Assert.Equal(HttpStatusCode.OK, deleteResponse.StatusCode);
        }

        private async Task<(string Token, string Cookie)> ExtractCsrfData(HttpClient client, string url)
        {
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var html = await response.Content.ReadAsStringAsync();
            var cookie = response.Headers.GetValues("Set-Cookie").FirstOrDefault() ?? string.Empty;

            var match = Regex.Match(html,
                @"<input[^>]*name=""__RequestVerificationToken""[^>]*value=""([^""]*)""",
                RegexOptions.IgnoreCase);

            return (match.Success ? match.Groups[1].Value : string.Empty, cookie);
        }

        private int ExtractFirstId(string html)
        {
            var match = Regex.Match(html, @"data-id=""(\d+)""");
            return match.Success ? int.Parse(match.Groups[1].Value) : -1;
        }
    }
}
