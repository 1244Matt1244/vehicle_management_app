using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Project.Service.Data.Context;
using Project.Service.Models;
using Xunit;

namespace Project.Tests.IntegrationTests
{
    public class DatabaseTests : IAsyncLifetime
    {
        private readonly ApplicationDbContext _context;

        public DatabaseTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);
        }

        public async Task InitializeAsync()
        {
            // Initialize required DbSets
            _context.VehicleMakes.Add(new VehicleMake 
            { 
                Id = 1, 
                Name = "Honda", 
                Abrv = "H", 
                VehicleModels = new List<VehicleModel>() 
            });
            await _context.SaveChangesAsync();
        }

        [Fact]
        public async Task AddMake_ShouldPersist()
        {
            var make = new VehicleMake 
            { 
                Id = 2, 
                Name = "Tesla", 
                Abrv = "T", 
                VehicleModels = new List<VehicleModel>() 
            };

            _context.VehicleMakes.Add(make);
            await _context.SaveChangesAsync();

            var savedMake = await _context.VehicleMakes.FindAsync(2);
            Assert.NotNull(savedMake);
        }

        public async Task DisposeAsync()
        {
            await _context.Database.EnsureDeletedAsync();
            await _context.DisposeAsync();
        }
    }
}