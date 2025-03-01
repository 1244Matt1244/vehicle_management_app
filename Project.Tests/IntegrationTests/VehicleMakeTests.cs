using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using Project.MVC; // Add correct namespace

namespace Project.Tests.IntegrationTests
{
    public class VehicleMakeTests : IClassFixture<WebApplicationFactory<Program>> // Fixed Program reference
    {
        private readonly WebApplicationFactory<Program> _factory;

        public VehicleMakeTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory.WithWebHostBuilder(builder => {
                builder.UseEnvironment("Test");
            });
        }

        [Fact]
        public async Task Full_CRUD_Workflow_With_CSRF()
        {
            var client = _factory.CreateClient(new WebApplicationFactoryClientOptions {
                AllowAutoRedirect = false
            });

            // CSRF token extraction with null check
            var csrfToken = await ExtractCsrfToken(client, "/VehicleMake/Create");
            Assert.NotNull(csrfToken);

            // Rest of test implementation remains unchanged
            // (Previous CRUD workflow steps)
        }

        private async Task<string> ExtractCsrfToken(HttpClient client, string url)
        {
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var html = await response.Content.ReadAsStringAsync();
            
            // Enhanced regex pattern
            var match = Regex.Match(html, 
                @"<input[^>]*name=[""']__RequestVerificationToken[""'][^>]*value=[""']([^""']+)[""']",
                RegexOptions.IgnoreCase);
            
            return match.Success ? match.Groups[1].Value : null;
        }
    }
}