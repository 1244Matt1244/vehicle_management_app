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
    public class VehicleMakeTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public VehicleMakeTests(WebApplicationFactory<Program> factory)
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
        public async Task Full_CRUD_Workflow_With_CSRF()
        {
            var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false,
                BaseAddress = new Uri("https://localhost")
            });

            // CREATE
            var csrfToken = await ExtractCsrfToken(client, "/VehicleMake/Create");
            Assert.NotNull(csrfToken);

            var createResponse = await client.PostAsync("/VehicleMake/Create", new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("__RequestVerificationToken", csrfToken),
                new KeyValuePair<string, string>("Name", "TestMake"),
                new KeyValuePair<string, string>("Abbreviation", "TMK")
            }));
            Assert.Equal(HttpStatusCode.Redirect, createResponse.StatusCode);

            // READ
            var indexResponse = await client.GetAsync("/VehicleMake");
            indexResponse.EnsureSuccessStatusCode();
            var indexContent = await indexResponse.Content.ReadAsStringAsync();
            var id = ExtractFirstId(indexContent);

            // UPDATE
            var editToken = await ExtractCsrfToken(client, $"/VehicleMake/Edit/{id}");
            var updateResponse = await client.PostAsync($"/VehicleMake/Edit/{id}", new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("__RequestVerificationToken", editToken),
                new KeyValuePair<string, string>("Name", "UpdatedMake"),
                new KeyValuePair<string, string>("Abbreviation", "UMK")
            }));
            Assert.Equal(HttpStatusCode.Redirect, updateResponse.StatusCode);

            // DELETE
            var deleteToken = await ExtractCsrfToken(client, $"/VehicleMake/Delete/{id}");
            var deleteResponse = await client.PostAsync($"/VehicleMake/Delete/{id}", new FormUrlEncodedContent(new[]
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

        private int ExtractFirstId(string html)
        {
            var match = Regex.Match(html, @"data-id=""(\d+)""");
            return match.Success ? int.Parse(match.Groups[1].Value) : -1;
        }
    }
}