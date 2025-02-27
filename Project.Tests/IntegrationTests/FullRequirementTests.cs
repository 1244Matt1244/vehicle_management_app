using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Project.Tests.IntegrationTests
{
    public class FullRequirementTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public FullRequirementTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Get_HomePage_ReturnsSuccess()
        {
            // Arrange
            var request = "/"; // Home page route

            // Act
            var response = await _client.GetAsync(request);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Get_AboutPage_ReturnsSuccess()
        {
            // Arrange
            var request = "/about"; // Update with your actual route

            // Act
            var response = await _client.GetAsync(request);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Post_ContactForm_ReturnsSuccess()
        {
            // Arrange
            var request = "/contact"; // Update with your actual route
            var content = new StringContent("{\"name\":\"Test User\", \"email\":\"test@example.com\"}", 
                Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync(request, content);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Get_NonExistentPage_ReturnsNotFound()
        {
            // Arrange
            var request = "/non-existent-page"; // Non-existent route

            // Act
            var response = await _client.GetAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Post_ContactForm_InvalidData_ReturnsBadRequest()
        {
            // Arrange
            var request = "/contact"; // Update with your actual route
            var content = new StringContent("{\"name\":\"\", \"email\":\"invalid-email\"}", 
                Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync(request, content);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Get_HealthCheck_ReturnsSuccess()
        {
            // Arrange
            var request = "/health"; // Health check route

            // Act
            var response = await _client.GetAsync(request);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        // Additional tests can be added here for other requirements
    }
}
