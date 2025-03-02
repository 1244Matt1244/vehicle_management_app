using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Project.Service.Data.Context;
using HtmlAgilityPack;
using System.Collections.Generic;
using System.Linq;
using Project.Service.Models;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Project.Tests.IntegrationTests
{
    public class VehicleMakeTests : IClassFixture<WebApplicationFactory<Program>>, IDisposable
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;
        private readonly ApplicationDbContext _dbContext;

        public VehicleMakeTests()
        {
            _factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.RemoveAll(typeof(DbContextOptions<ApplicationDbContext>));
                    services.AddDbContext<ApplicationDbContext>(options => 
                        options.UseInMemoryDatabase("TestDB_" + Guid.NewGuid()));
                });
            });
            
            _client = _factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false,
                HandleCookies = true
            });
            
            var scope = _factory.Services.CreateScope();
            _dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            _dbContext.Database.EnsureCreated();
        }

        [Fact]
        public async Task Full_CRUD_Workflow_With_CSRF()
        {
            // Get CSRF token
            var createPage = await _client.GetAsync("/VehicleMake/Create");
            var (csrfToken, cookie) = await ExtractCsrfData(createPage);
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
            var created = await _dbContext.VehicleMakes.FirstAsync();
            var id = created.Id;

            // Edit
            var editPage = await _client.GetAsync($"/VehicleMake/Edit/{id}");
            var (editCsrf, _) = await ExtractCsrfData(editPage);
            
            var updateResponse = await _client.PostAsync($"/VehicleMake/Edit/{id}", new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("__RequestVerificationToken", editCsrf),
                new KeyValuePair<string, string>("Name", "UpdatedMake"),
                new KeyValuePair<string, string>("Abbreviation", "UMK"),
                new KeyValuePair<string, string>("Id", id.ToString())
            }));
            Assert.Equal(HttpStatusCode.Redirect, updateResponse.StatusCode);

            // Delete
            var deletePage = await _client.GetAsync($"/VehicleMake/Delete/{id}");
            var (deleteCsrf, _) = await ExtractCsrfData(deletePage);
            
            var deleteResponse = await _client.PostAsync($"/VehicleMake/Delete/{id}", new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("__RequestVerificationToken", deleteCsrf),
                new KeyValuePair<string, string>("Id", id.ToString())
            }));
            Assert.Equal(HttpStatusCode.Redirect, deleteResponse.StatusCode);
        }

        private async Task<(string Token, string Cookie)> ExtractCsrfData(HttpResponseMessage response)
        {
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            
            var doc = new HtmlDocument();
            doc.LoadHtml(content);
            
            var tokenNode = doc.DocumentNode.SelectSingleNode("//input[@name='__RequestVerificationToken']") 
                        ?? throw new Exception($"CSRF token not found in:\n{content}");
            
            return (
                Token: tokenNode.GetAttributeValue("value", ""),
                Cookie: response.Headers.GetValues("Set-Cookie").FirstOrDefault() ?? ""
            );
        }

        public void Dispose()
        {
            _dbContext.Database.EnsureDeleted();
            _client.Dispose();
            _factory.Dispose();
        }
    }
}