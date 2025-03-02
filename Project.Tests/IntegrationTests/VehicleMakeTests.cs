using System;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Project.Service.Data.Context;
using System.Collections.Generic;
using System.Linq;

namespace Project.Tests.IntegrationTests
{
    public class VehicleMakeTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public VehicleMakeTests()
        {
            _factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    // Fix RemoveAll error
                    var descriptor = services.FirstOrDefault(d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
                    if (descriptor != null) services.Remove(descriptor);

                    services.AddDbContext<ApplicationDbContext>(options => 
                        options.UseInMemoryDatabase("TestDB"));
                });
            });
        }

        [Fact]
        public async Task Full_CRUD_Workflow_With_CSRF()
        {
            var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false,
                BaseAddress = new Uri("https://localhost")
            });

            // Get CSRF token with cookie handling
            var initialResponse = await client.GetAsync("/VehicleMake/Create");
            initialResponse.EnsureSuccessStatusCode();
            var (csrfToken, cookie) = await ExtractCsrfData(initialResponse);

            client.DefaultRequestHeaders.Add("Cookie", cookie);

            // Create
            var createResponse = await client.PostAsync("/VehicleMake/Create", new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("__RequestVerificationToken", csrfToken),
                new KeyValuePair<string, string>("Name", "TestMake"),
                new KeyValuePair<string, string>("Abbreviation", "TMK")
            }));
            
            // Fix status code expectation
            Assert.Equal(HttpStatusCode.Redirect, createResponse.StatusCode);

            // Read
            var indexResponse = await client.GetAsync("/VehicleMake");
            indexResponse.EnsureSuccessStatusCode();
            var indexContent = await indexResponse.Content.ReadAsStringAsync();
            var id = ExtractFirstId(indexContent);
            Assert.True(id > 0, "No valid ID found after creation.");

            // Update
            var editResponse = await client.GetAsync($"/VehicleMake/Edit/{id}");
            editResponse.EnsureSuccessStatusCode();
            var (editToken, _) = await ExtractCsrfData(editResponse);
            
            var updateResponse = await client.PostAsync($"/VehicleMake/Edit/{id}", new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("__RequestVerificationToken", editToken),
                new KeyValuePair<string, string>("Name", "UpdatedMake"),
                new KeyValuePair<string, string>("Abbreviation", "UMK")
            }));
            Assert.Equal(HttpStatusCode.Redirect, updateResponse.StatusCode);

            // Delete
            var deleteResponse = await client.PostAsync($"/VehicleMake/Delete/{id}", new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("__RequestVerificationToken", editToken)
            }));
            Assert.Equal(HttpStatusCode.Redirect, deleteResponse.StatusCode);
        }

        private async Task<(string Token, string Cookie)> ExtractCsrfData(HttpResponseMessage response)
        {
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
