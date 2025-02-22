using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using Project.Service.Data.Context;
using Project.Service.Data.DTOs;
using Project.Service.Data.Helpers;
using Project.Service.Interfaces;
using Project.Service.Mappings;
using Project.Service.Models;
using Project.Service.Services;
using Xunit;

namespace Project.Tests
{
    public class VehicleServiceTests
    {
        private readonly IMapper _mapper;
        private readonly DbContextOptions<ApplicationDbContext> _options;

        public VehicleServiceTests()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<ServiceMappingProfile>());
            _mapper = config.CreateMapper();
            _options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDB")
                .Options;
        }

        [Fact]
        public async Task CreateMakeAsync_ValidData_CreatesRecord()
        {
            using var context = new ApplicationDbContext(_options);
            var service = new VehicleService(context, _mapper);
            
            await service.CreateMakeAsync(new VehicleMakeDTO { Name = "TestMake" });
            
            Assert.Single(context.VehicleMakes);
        }

        [Fact]
        public async Task GetMakesAsync_ReturnsPaginatedResults()
        {
            using var context = new ApplicationDbContext(_options);
            var service = new VehicleService(context, _mapper);
            
            context.VehicleMakes.AddRange(
                new VehicleMake { Name = "Make1" },
                new VehicleMake { Name = "Make2" }
            );
            await context.SaveChangesAsync();

            var result = await service.GetMakesAsync(1, 10, "Name", "asc", "");
            
            Assert.Equal(2, result.Items.Count);
        }
    }
}
