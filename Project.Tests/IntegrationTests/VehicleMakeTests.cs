using System;
using System.Collections.Generic;
using System.Linq;
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
using Project.Service.Models;

namespace Project.Tests.IntegrationTests
{
    public class VehicleMakeTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public VehicleMakeTests(WebApplicationFactory<Program> factory)
        {
            // Configure the in-memory database for testing to avoid using the actual database
            _factory = factory.WithWebHostBuilder(builder => 
            {
                builder.ConfigureServices(services => 
                {
                    // Remove the existing DbContext registration if present
                    var descriptor = services.SingleOrDefault(
                        d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
                    if (descriptor != null)
                    {
                        services.Remove(descriptor);
                    }

                    // Add an in-memory database for testing purposes
                    services.AddDbContext<ApplicationDbContext>(options => 
                        options.UseInMemoryDatabase($"TestDB_{Guid.NewGuid()}"));
                });
            });
        }

        [Fact]
        public async Task Full_CRUD_Workflow_With_CSRF()
        {
            var client = _factory.CreateClient(new WebApplicationFactoryClientOptions 
            {
                AllowAutoRedirect = true,
                HandleCookies = true
            });

            // Create - POST request to create a new vehicle make
            var csrfToken = await ExtractCsrfToken(client, "/VehicleMake/Create");
            var createResponse = await client.PostAsync("/VehicleMake/Create", new FormUrlEncodedContent(new[] 
            {
                new KeyValuePair<string, string>("Name", "TestMake"),
                new KeyValuePair<string, string>("Abbreviation", "TMK"),
                new KeyValuePair<string, string>("__RequestVerificationToken", csrfToken)
            }));
            Assert.Equal(HttpStatusCode.OK, createResponse.StatusCode);

            // Extract ID from the index page to use for update and delete operations
            var indexHtml = await client.GetStringAsync("/VehicleMake");
            var idMatch = Regex.Match(indexHtml, @"data-id=""(\d+)""");
            Assert.True(idMatch.Success, "No ID found in the index page.");
            var id = idMatch.Groups[1].Value;

            // Update - POST request to update the vehicle make
            var editToken = await ExtractCsrfToken(client, $"/VehicleMake/Edit/{id}");
            var updateResponse = await client.PostAsync($"/VehicleMake/Edit/{id}", new FormUrlEncodedContent(new[] 
            {
                new KeyValuePair<string, string>("Name", "UpdatedMake"),
                new KeyValuePair<string, string>("Abbreviation", "UMK"),
                new KeyValuePair<string, string>("__RequestVerificationToken", editToken)
            }));
            Assert.Equal(HttpStatusCode.OK, updateResponse.StatusCode);

            // Verify Update - Ensure the updated name appears in the HTML response
            var updatedHtml = await client.GetStringAsync("/VehicleMake");
            Assert.Contains("UpdatedMake", updatedHtml);

            // Delete - POST request to delete the vehicle make
            var deleteToken = await ExtractCsrfToken(client, $"/VehicleMake/Delete/{id}");
            var deleteResponse = await client.PostAsync($"/VehicleMake/Delete/{id}", new FormUrlEncodedContent(new[] 
            {
                new KeyValuePair<string, string>("__RequestVerificationToken", deleteToken)
            }));
            Assert.Equal(HttpStatusCode.OK, deleteResponse.StatusCode);
        }

        // Method to extract CSRF token from the response page
        private async Task<string> ExtractCsrfToken(HttpClient client, string url)
        {
            // Send GET request to fetch the page HTML
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var html = await response.Content.ReadAsStringAsync();

            // Use regex to extract the CSRF token from the HTML
            var match = Regex.Match(html, 
                @"<input[^>]*name=[""']__RequestVerificationToken[""'][^>]*value=[""']([^""']+)[""']",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

            // Return the CSRF token if found, else throw an exception
            return match.Success 
                ? match.Groups[1].Value
                : throw new Exception($"CSRF token not found at {url}\nHTML:{html}");
        }
    }
}
