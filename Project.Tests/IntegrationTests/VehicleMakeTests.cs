using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace IntegrationTests
{
    public class VehicleMakeTests : IClassFixture<WebApplicationFactory<YourNamespace.Startup>>
    {
        private readonly WebApplicationFactory<YourNamespace.Startup> _factory;

        public VehicleMakeTests(WebApplicationFactory<YourNamespace.Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Full_CRUD_Workflow_With_CSRF()
        {
            // Arrange
            var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

            // Act 1: Create
            var csrf = await ExtractCsrfToken(client, "/VehicleMake/Create");
            var postResponse = await client.PostAsync("/VehicleMake/Create", new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("Name", "TestMake"),
                new KeyValuePair<string, string>("Abbreviation", "TMK"),
                new KeyValuePair<string, string>("__RequestVerificationToken", csrf)
            }));

            // Assert 1
            Assert.Equal(HttpStatusCode.Redirect, postResponse.StatusCode);

            // Act 2: Verify & Extract ID
            var indexHtml = await client.GetStringAsync("/VehicleMake");
            var id = Regex.Match(indexHtml, @"data-id=""(\d+)""").Groups[1].Value;

            // Act 3: Update
            var editCsrf = await ExtractCsrfToken(client, $"/VehicleMake/Edit/{id}");
            var putResponse = await client.PostAsync($"/VehicleMake/Edit/{id}", new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("Name", "UpdatedMake"),
                new KeyValuePair<string, string>("Abbreviation", "UMK"),
                new KeyValuePair<string, string>("__RequestVerificationToken", editCsrf)
            }));

            // Assert 3
            Assert.Equal(HttpStatusCode.Redirect, putResponse.StatusCode);

            // Act 4: Delete
            var deleteCsrf = await ExtractCsrfToken(client, $"/VehicleMake/Delete/{id}");
            var deleteResponse = await client.PostAsync($"/VehicleMake/Delete/{id}", new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("__RequestVerificationToken", deleteCsrf)
            }));

            // Assert 4
            Assert.Equal(HttpStatusCode.Redirect, deleteResponse.StatusCode);
        }

        private static async Task<string> ExtractCsrfToken(HttpClient client, string url)
        {
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode(); // Ensure the response is successful
            var html = await response.Content.ReadAsStringAsync();
            return Regex.Match(html, @"name=""__RequestVerificationToken"" value=""([^""]+)""").Groups[1].Value;
        }
    }
}
