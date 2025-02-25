using System;
using System.Collections.Generic; // Added
using System.Threading.Tasks;     // Added
using Microsoft.EntityFrameworkCore;
using Project.Service.Data.Context;
using Project.Service.Models;
using Xunit;
using Assert = Xunit.Assert; // Resolve the ambiguity
using Microsoft.AspNetCore.Mvc;  // For ViewResult
using Project.Service.Mappings;  // For ServiceMappingProfile
using Project.Service.Data.Helpers;    // For PaginatedList<>
using Project.MVC.ViewModels;    // For VehicleMakeVM

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

        // Implement IAsyncLifetime correctly
        public async Task InitializeAsync() // Changed to Task
        {
            await _context.Database.EnsureCreatedAsync();
            
            // Seed test data
            _context.VehicleMakes.Add(new VehicleMake 
            { 
                Id = 1, 
                Name = "Honda", 
                Abbreviation = "H",
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
                Abbreviation = "T",
                VehicleModels = new List<VehicleModel>() 
            };

            _context.VehicleMakes.Add(make);
            await _context.SaveChangesAsync();

            var savedMake = await _context.VehicleMakes.FindAsync(2);
            Assert.NotNull(savedMake);
        }

        public async Task DisposeAsync() // Changed to Task
        {
            await _context.Database.EnsureDeletedAsync();
            await _context.DisposeAsync();
        }
    }
}