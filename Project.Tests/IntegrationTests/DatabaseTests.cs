using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Project.Service.Data.Context;
using Project.Service.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Project.Tests.IntegrationTests
{
    [TestClass]
    public class DatabaseTests
    {
        private ApplicationDbContext _context;

        [TestInitialize]
        public async Task InitializeAsync()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);
            await _context.Database.EnsureCreatedAsync();
            
            // Seed test data
            _context.VehicleMakes.Add(new VehicleMake 
            { 
                Id = 1, 
                Name = "Honda", 
                Abrv = "H",
                VehicleModels = new List<VehicleModel>() 
            });
            await _context.SaveChangesAsync();
        }

        [TestMethod]
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
            Assert.IsNotNull(savedMake);
        }

        [TestCleanup]
        public async Task CleanupAsync()
        {
            await _context.Database.EnsureDeletedAsync();
            await _context.DisposeAsync();
        }
    }
}
