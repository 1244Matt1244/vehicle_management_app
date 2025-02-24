// Project.Tests/Helpers/TestHelpers.cs
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Project.Service.Data.Context;
using Project.Service.Models;
using Project.Service.Mappings;
using System.Collection.Generic;


namespace Project.Tests.Helpers
{
    public static class TestHelpers
    {
        public static ApplicationDbContext CreateTestDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite("Data Source=:memory:;Cache=Shared")
                .Options;

            var context = new ApplicationDbContext(options);
            context.Database.OpenConnection();
            context.Database.EnsureCreated();

            // Add test data
            context.VehicleMakes.AddRange(TestData.GetVehicleMakes());
            context.VehicleModels.AddRange(TestData.GetVehicleModels());
            context.SaveChanges();

            return context;
        }

        public static IMapper CreateTestMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ServiceMappingProfile>();
            });
            return config.CreateMapper();
        }
    }

    public static class TestData
    {
        public static List<VehicleMake> GetVehicleMakes() => new()
        {
            new VehicleMake { Id = 1, Name = "Ford", Abrv = "F" },
            new VehicleMake { Id = 2, Name = "BMW", Abrv = "B" }
        };

        public static List<VehicleModel> GetVehicleModels() => new()
        {
            new VehicleModel { Id = 1, Name = "Focus", Abrv = "F", MakeId = 1 },
            new VehicleModel { Id = 2, Name = "X5", Abrv = "X5", MakeId = 2 }
        };
    }
}