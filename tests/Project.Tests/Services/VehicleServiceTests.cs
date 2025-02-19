using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Project.Service.Data;
using Project.Service.DTOs;
using Project.Service.Interfaces;
using Project.Service.Mapping;
using Project.Service.Models;
using Project.Service.Services;
using Xunit;

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

        // VehicleMake Tests
        [Fact]
        public async Task CreateMakeAsync_AddsNewMake()
        {
            var makeDto = new VehicleMakeDTO { Name = "Test Make", Abrv = "TM" };
            await _service.AddMakeAsync(makeDto);

            var createdMake = await _context.VehicleMakes.FirstOrDefaultAsync(m => m.Name == "Test Make");
            Assert.NotNull(createdMake);
            Assert.Equal("TM", createdMake.Abrv);
        }

        [Fact]
        public async Task GetMakeByIdAsync_ReturnsCorrectMake()
        {
            var make = new VehicleMake { Name = "Test Make", Abrv = "TM" };
            _context.VehicleMakes.Add(make);
            await _context.SaveChangesAsync();

            var result = await _service.GetMakeByIdAsync(make.Id);

            Assert.NotNull(result);
            Assert.Equal("Test Make", result.Name);
            Assert.Equal("TM", result.Abrv);
        }

        [Fact]
        public async Task UpdateMakeAsync_UpdatesMake()
        {
            var make = new VehicleMake { Name = "Old Name", Abrv = "ON" };
            _context.VehicleMakes.Add(make);
            await _context.SaveChangesAsync();

            var updatedMakeDto = new VehicleMakeDTO { Id = make.Id, Name = "Updated Name", Abrv = "UN" };
            await _service.UpdateMakeAsync(updatedMakeDto);

            var updatedMake = await _context.VehicleMakes.FindAsync(make.Id);
            Assert.NotNull(updatedMake);
            Assert.Equal("Updated Name", updatedMake.Name);
            Assert.Equal("UN", updatedMake.Abrv);
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

        [Fact]
        public async Task GetAllMakesAsync_ReturnsFilteredAndSortedMakes()
        {
            _context.VehicleMakes.AddRange(
                new VehicleMake { Name = "BMW", Abrv = "BMW" },
                new VehicleMake { Name = "Audi", Abrv = "AUD" },
                new VehicleMake { Name = "Volkswagen", Abrv = "VW" }
            );
            await _context.SaveChangesAsync();

            var result = await _service.GetAllMakesAsync("A", "name_desc", 1, 10);

            Assert.NotNull(result);
            Assert.Single(result); // Only "Audi" matches the filter
            Assert.Equal("Audi", result.First().Name);
        }

        [Fact]
        public async Task GetAllMakesAsync_ReturnsPagedResults()
        {
            _context.VehicleMakes.AddRange(
                new VehicleMake { Name = "Make1", Abrv = "M1" },
                new VehicleMake { Name = "Make2", Abrv = "M2" },
                new VehicleMake { Name = "Make3", Abrv = "M3" }
            );
            await _context.SaveChangesAsync();

            var result = await _service.GetAllMakesAsync(null, null, 2, 2); // Page 2, Page Size 2

            Assert.NotNull(result);
            Assert.Single(result); // Only one item on page 2
            Assert.Equal("Make3", result.First().Name);
        }

        // VehicleModel Tests
        [Fact]
        public async Task CreateModelAsync_AddsNewModel()
        {
            var make = new VehicleMake { Name = "Test Make", Abrv = "TM" };
            _context.VehicleMakes.Add(make);
            await _context.SaveChangesAsync();

            var modelDto = new VehicleModelDTO { Name = "Test Model", Abrv = "TM", MakeId = make.Id };
            await _service.AddModelAsync(modelDto);

            var createdModel = await _context.VehicleModels.FirstOrDefaultAsync(m => m.Name == "Test Model");
            Assert.NotNull(createdModel);
            Assert.Equal("TM", createdModel.Abrv);
            Assert.Equal(make.Id, createdModel.MakeId);
        }

        [Fact]
        public async Task GetModelByIdAsync_ReturnsCorrectModel()
        {
            var make = new VehicleMake { Name = "Test Make", Abrv = "TM" };
            var model = new VehicleModel { Name = "Test Model", Abrv = "TM", VehicleMake = make };
            _context.VehicleModels.Add(model);
            await _context.SaveChangesAsync();

            var result = await _service.GetModelByIdAsync(model.Id);

            Assert.NotNull(result);
            Assert.Equal("Test Model", result.Name);
            Assert.Equal("TM", result.Abrv);
            Assert.Equal("Test Make", result.MakeName);
        }

        [Fact]
        public async Task UpdateModelAsync_UpdatesModel()
        {
            var make = new VehicleMake { Name = "Test Make", Abrv = "TM" };
            var model = new VehicleModel { Name = "Old Model", Abrv = "OM", VehicleMake = make };
            _context.VehicleModels.Add(model);
            await _context.SaveChangesAsync();

            var updatedModelDto = new VehicleModelDTO { Id = model.Id, Name = "Updated Model", Abrv = "UM", MakeId = make.Id };
            await _service.UpdateModelAsync(updatedModelDto);

            var updatedModel = await _context.VehicleModels.FindAsync(model.Id);
            Assert.NotNull(updatedModel);
            Assert.Equal("Updated Model", updatedModel.Name);
            Assert.Equal("UM", updatedModel.Abrv);
        }

        [Fact]
        public async Task DeleteModelAsync_RemovesModel()
        {
            var make = new VehicleMake { Name = "Test Make", Abrv = "TM" };
            var model = new VehicleModel { Name = "Test Model", Abrv = "TM", VehicleMake = make };
            _context.VehicleModels.Add(model);
            await _context.SaveChangesAsync();

            await _service.DeleteModelAsync(model.Id);

            var deletedModel = await _context.VehicleModels.FindAsync(model.Id);
            Assert.Null(deletedModel);
        }

        [Fact]
        public async Task GetAllModelsAsync_ReturnsFilteredAndSortedModels()
        {
            var make = new VehicleMake { Name = "Test Make", Abrv = "TM" };
            _context.VehicleModels.AddRange(
                new VehicleModel { Name = "Model1", Abrv = "M1", VehicleMake = make },
                new VehicleModel { Name = "Model2", Abrv = "M2", VehicleMake = make },
                new VehicleModel { Name = "AnotherModel", Abrv = "AM", VehicleMake = make }
            );
            await _context.SaveChangesAsync();

            var result = await _service.GetAllModelsAsync("Model", "name_desc", 1, 10);

            Assert.NotNull(result);
            Assert.Equal(2, result.Count()); // Only "Model1" and "Model2" match the filter
            Assert.Equal("Model2", result.First().Name); // Sorted descending by name
        }

        [Fact]
        public async Task GetAllModelsAsync_ReturnsPagedResults()
        {
            var make = new VehicleMake { Name = "Test Make", Abrv = "TM" };
            _context.VehicleModels.AddRange(
                new VehicleModel { Name = "Model1", Abrv = "M1", VehicleMake = make },
                new VehicleModel { Name = "Model2", Abrv = "M2", VehicleMake = make },
                new VehicleModel { Name = "Model3", Abrv = "M3", VehicleMake = make }
            );
            await _context.SaveChangesAsync();

            var result = await _service.GetAllModelsAsync(null, null, 2, 2); // Page 2, Page Size 2

            Assert.NotNull(result);
            Assert.Single(result); // Only one item on page 2
            Assert.Equal("Model3", result.First().Name);
        }
    }
}
