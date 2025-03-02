using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Project.Service.Data.Context;
using HtmlAgilityPack;
using System.Net.Http;
using System.Collections.Generic;
using System.Linq;
using Project.Service.Models;
using Microsoft.AspNetCore.Hosting;

namespace Project.Tests.IntegrationTests
{
    public class FullRequirementTests : IClassFixture<WebApplicationFactory<Program>>, IDisposable
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;
        private readonly ApplicationDbContext _dbContext;

        public FullRequirementTests()
        {
            _factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
            {
                builder.UseEnvironment("Test");
                builder.ConfigureServices(services =>
                {
                    services.RemoveAll(typeof(DbContextOptions<ApplicationDbContext>));
                    services.AddDbContext<ApplicationDbContext>(options => 
                        options.UseInMemoryDatabase("FullReqTestDB_"+Guid.NewGuid()));
                });
                
                builder.Configure(app =>
                {
                    app.Use(async (context, next) =>
                    {
                        context.Request.Scheme = "https";
                        await next();
                    });
                });
            });
            
            _client = _factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                HandleCookies = true,
                AllowAutoRedirect = false,
                BaseAddress = new Uri("https://localhost")
            });
            
            var scope = _factory.Services.CreateScope();
            _dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            _dbContext.Database.EnsureCreated();
        }

        [Fact]
        public async Task CRUD_Workflow_ReturnsProperStatusCodes()
        {
            // Get CSRF token and cookie
            var (csrfToken, cookie) = await GetCsrfData("/VehicleMake/Create");
            _client.DefaultRequestHeaders.Add("Cookie", cookie);

            // Create
            var createResponse = await _client.PostAsync("/VehicleMake/Create", new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("__RequestVerificationToken", csrfToken),
                new KeyValuePair<string, string>("Name", "TestMake"),
                new KeyValuePair<string, string>("Abbreviation", "TMK")
            }));
            
            Assert.Equal(HttpStatusCode.Redirect, createResponse.StatusCode);

            // Verify creation
            var created = _dbContext.VehicleMakes.FirstOrDefault();
            Assert.NotNull(created);
            var id = created.Id;

            // Read details
            var detailsResponse = await _client.GetAsync($"/VehicleMake/Details/{id}");
            Assert.Equal(HttpStatusCode.OK, detailsResponse.StatusCode);

            // Update
            var (editCsrf, _) = await GetCsrfData($"/VehicleMake/Edit/{id}");
            var updateResponse = await _client.PostAsync($"/VehicleMake/Edit/{id}", new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("__RequestVerificationToken", editCsrf),
                new KeyValuePair<string, string>("Name", "UpdatedMake"),
                new KeyValuePair<string, string>("Abbreviation", "UMK"),
                new KeyValuePair<string, string>("Id", id.ToString())
            }));
            Assert.Equal(HttpStatusCode.Redirect, updateResponse.StatusCode);

            // Delete
            var (deleteCsrf, _) = await GetCsrfData($"/VehicleMake/Delete/{id}");
            var deleteResponse = await _client.PostAsync($"/VehicleMake/Delete/{id}", new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("__RequestVerificationToken", deleteCsrf),
                new KeyValuePair<string, string>("Id", id.ToString())
            }));
            Assert.Equal(HttpStatusCode.Redirect, deleteResponse.StatusCode);
        }

        private async Task<(string Token, string Cookie)> GetCsrfData(string url)
        {
            var response = await _client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            
            var content = await response.Content.ReadAsStringAsync();
            var doc = new HtmlDocument();
            doc.LoadHtml(content);

            var tokenNode = doc.DocumentNode.SelectSingleNode("//input[@name='__RequestVerificationToken']") 
                        ?? throw new Exception($"CSRF token not found in:\n{content}");
            
            var cookie = response.Headers.GetValues("Set-Cookie").FirstOrDefault();
            return (tokenNode.GetAttributeValue("value", ""), cookie ?? "");
        }

        public void Dispose()
        {
            _dbContext.Database.EnsureDeleted();
            _client.Dispose();
            _factory.Dispose();
        }
    }
}