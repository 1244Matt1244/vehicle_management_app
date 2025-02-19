using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;
using Project.Service.Data.Context;
using Project.Service.Data.DTOs;
using Project.Service.Interfaces;
using Project.Service.Mapping;
using Project.Service.Models;
using Project.Service.Services;

namespace Project.Tests.Services
{
    public class VehicleServiceTests
    {
        private readonly ApplicationDbContext _context;
        private readonly IVehicleService _service;

        public VehicleServiceTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);

            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ServiceMappingProfile>();
            });
            var mapper = mapperConfig.CreateMapper();

            _service = new VehicleService(_context, mapper);
        }

        [Fact]
        public async Task CreateMakeAsync_AddsNewMake()
        {
            var makeDto = new VehicleMakeDTO { Name = "Test Make", Abrv = "TM" };
            await _service.CreateMakeAsync(makeDto);

            var createdMake = await _context.VehicleMakes.FirstOrDefaultAsync(m => m.Name == "Test Make");
            Assert.NotNull(createdMake);
        }

        [Fact]
        public async Task DeleteMakeAsync_RemovesMake()
        {
            var make = new VehicleMake { Name = "Test Make", Abrv = "TM" };
            _context.VehicleMakes.Add(make);
            await _context.SaveChangesAsync();

            await _service.DeleteMakeAsync(make.Id);

            var deletedMake = await _context.VehicleMakes.FindAsync(make.Id);
            Assert.Null(deletedMake);
        }

        // Add similar tests for VehicleModel...
    }
}