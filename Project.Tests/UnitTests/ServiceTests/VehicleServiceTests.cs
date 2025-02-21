using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Project.Service.Data.Context;
using Project.Service.Data.DTOs;
using Project.Service.Models;
using Project.Service.Services;
using Xunit;

namespace Project.Tests.UnitTests.ServiceTests
{
    public class VehicleServiceTests
    {
        private readonly IMapper _mapper;

        public VehicleServiceTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<VehicleMakeDTO, VehicleMake>();
                cfg.CreateMap<VehicleMake, VehicleMakeDTO>();
            });

            _mapper = config.CreateMapper();
        }

        [Fact]
        public async Task CreateMakeAsync_AddsMake()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("CreateMakeTestDB")
                .Options;

            using var context = new ApplicationDbContext(options);
            var service = new VehicleService(context, _mapper);

            var makeDto = new VehicleMakeDTO { Name = "TestMake", Abrv = "TM" };

            // Act
            await service.CreateMakeAsync(makeDto);

            // Assert
            var make = await context.VehicleMakes.FirstOrDefaultAsync(m => m.Name == "TestMake");
            Assert.NotNull(make);
            Assert.Equal("TestMake", make.Name);
        }

        [Fact]
        public async Task GetMakesAsync_ReturnsMakes()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("GetMakesTestDB")
                .Options;

            using var context = new ApplicationDbContext(options);
            context.VehicleMakes.Add(new VehicleMake { Name = "Make1", Abrv = "M1" });
            context.VehicleMakes.Add(new VehicleMake { Name = "Make2", Abrv = "M2" });
            await context.SaveChangesAsync();

            var service = new VehicleService(context, _mapper);

            // Act
            var makes = await service.GetMakesAsync(1, 10, "Name", "asc", "");

            // Assert
            Assert.NotNull(makes);
            Assert.Equal(2, makes.Count);
        }

        [Fact]
        public async Task DeleteMakeAsync_DeletesMake()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("DeleteMakeTestDB")
                .Options;

            using var context = new ApplicationDbContext(options);
            var make = new VehicleMake { Id = 1, Name = "TestMake", Abrv = "TM" };
            context.VehicleMakes.Add(make);
            await context.SaveChangesAsync();

            var service = new VehicleService(context, _mapper);

            // Act
            await service.DeleteMakeAsync(1);

            // Assert
            var deletedMake = await context.VehicleMakes.FindAsync(1);
            Assert.Null(deletedMake);
        }
    }
}
