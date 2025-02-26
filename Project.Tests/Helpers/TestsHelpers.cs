using AutoMapper;
using Project.MVC.Helpers;
using Project.MVC.Mappings;
using Project.Service.Mappings;
using Project.Service.Models;

namespace Project.Tests
{
    public static class TestHelpers
    {
        public static IMapper CreateTestMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ServiceMappingProfile>();
                cfg.AddProfile<MvcMappingProfile>();
            });
            return config.CreateMapper();
        }

        public static VehicleMake CreateTestMake(int id = 1) => new()
        {
            Id = id,
            Name = $"Make{id}",
            Abbreviation = $"M{id}"
        };
    }
}