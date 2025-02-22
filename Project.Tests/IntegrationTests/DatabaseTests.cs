using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Project.Service.Data.Context;
using Project.Service.Models; 
using Xunit;

namespace Project.Tests
{
    public class DatabaseTests : IAsyncLifetime
    {
        private ApplicationDbContext _context;
        private const string TestDbName = "TestDb_" + Guid.NewGuid();

        public async Task InitializeAsync()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: TestDbName)
                .Options;

            _context = new ApplicationDbContext(options);
            await SeedTestDataAsync();
        }

        private async Task SeedTestDataAsync()
        {
            await _context.VehicleMakes.AddAsync(new VehicleMake { Name = "Honda" });
            await _context.SaveChangesAsync();
        }

        [Fact]
        public async Task AddMake_ShouldPersistInDatabase()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb_" + Guid.NewGuid())
                .Options;

            // Act & Assert
            using (var context = new ApplicationDbContext(options))
            {
                var make = new VehicleMake { Name = "Tesla" };
                await context.VehicleMakes.AddAsync(make);
                await context.SaveChangesAsync();
            }

            using (var context = new ApplicationDbContext(options))
            {
                Assert.Equal(1, await context.VehicleMakes.CountAsync());
                Assert.NotNull(await context.VehicleMakes.FirstOrDefaultAsync(m => m.Name == "Tesla"));
            }
        }

        public async Task DisposeAsync()
        {
            await _context.Database.EnsureDeletedAsync();
            await _context.DisposeAsync();
        }
    }
}