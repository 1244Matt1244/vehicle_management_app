using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using Project.MVC;

namespace Project.Tests.IntegrationTests
{
    public class FullRequirementTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly WebApplicationFactory<Program> _factory;

        public FullRequirementTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory.WithWebHostBuilder(builder =>
            {
                builder.UseSetting("ConnectionStrings:DefaultConnection", "TestConnectionString");
            });

            _client = _factory.CreateClient();
        }

        [Fact]
        public async Task CRUD_Workflow_ReturnsProperStatusCodes()
        {
            // Step 1: Get Create Form
            var getCreateResponse = await _client.GetAsync("/VehicleMake/Create");
            getCreateResponse.EnsureSuccessStatusCode();
            var createFormContent = await getCreateResponse.Content.ReadAsStringAsync();
            var verificationToken = ExtractAntiForgeryToken(createFormContent);
            Assert.NotNull(verificationToken);

            // Step 2: Create New VehicleMake
            var postContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("Name", "TestMake"),
                new KeyValuePair<string, string>("Abbreviation", "TM"),
                new KeyValuePair<string, string>("__RequestVerificationToken", verificationToken!)
            });

            var postResponse = await _client.PostAsync("/VehicleMake/Create", postContent);
            Assert.Equal(HttpStatusCode.Redirect, postResponse.StatusCode);

            // Step 3: Follow Redirect to Index
            var indexResponse = await _client.GetAsync(postResponse.Headers.Location);
            indexResponse.EnsureSuccessStatusCode();
            var indexContent = await indexResponse.Content.ReadAsStringAsync();

            Console.WriteLine(indexContent); // Debug: Check HTML content structure

            var id = ExtractFirstIdFromTable(indexContent);
            Assert.NotNull(id);

            // Step 4: Get Edit Form
            var getEditResponse = await _client.GetAsync($"/VehicleMake/Edit/{id}");
            getEditResponse.EnsureSuccessStatusCode();
            var editFormContent = await getEditResponse.Content.ReadAsStringAsync();
            var editVerificationToken = ExtractAntiForgeryToken(editFormContent);
            Assert.NotNull(editVerificationToken);

            // Step 5: Update VehicleMake
            var putContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("Id", id!),
                new KeyValuePair<string, string>("Name", "UpdatedMake"),
                new KeyValuePair<string, string>("Abbreviation", "UM"),
                new KeyValuePair<string, string>("__RequestVerificationToken", editVerificationToken!)
            });

            var putResponse = await _client.SendAsync(new HttpRequestMessage(HttpMethod.Post, $"/VehicleMake/Edit/{id}")
            {
                Content = putContent
            });
            Assert.Equal(HttpStatusCode.Redirect, putResponse.StatusCode);

            // Step 6: Get Delete Confirmation
            var getDeleteResponse = await _client.GetAsync($"/VehicleMake/Delete/{id}");
            getDeleteResponse.EnsureSuccessStatusCode();
            var deleteFormContent = await getDeleteResponse.Content.ReadAsStringAsync();
            var deleteVerificationToken = ExtractAntiForgeryToken(deleteFormContent);
            Assert.NotNull(deleteVerificationToken);

            // Step 7: Delete VehicleMake
            var deleteContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("Id", id!),
                new KeyValuePair<string, string>("__RequestVerificationToken", deleteVerificationToken!)
            });

            var deleteResponse = await _client.PostAsync($"/VehicleMake/Delete/{id}", deleteContent);
            Assert.Equal(HttpStatusCode.Redirect, deleteResponse.StatusCode);
        }

        private string? ExtractAntiForgeryToken(string htmlContent)
        {
            var match = Regex.Match(htmlContent, @"name=""__RequestVerificationToken""[^>]*value=""([^""]+)""", RegexOptions.IgnoreCase);
            return match.Success ? match.Groups[1].Value : null;
        }

        private string? ExtractFirstIdFromTable(string htmlContent)
        {
            var match = Regex.Match(htmlContent, @"<tr[^>]*>\s*<td[^>]*>(\d+)</td>", RegexOptions.Singleline);
            return match.Success ? match.Groups[1].Value : null;
        }
    }
}
