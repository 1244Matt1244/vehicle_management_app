using Project.Service.Models;
using AutoMapper;
using Project.Service.Mappings;

namespace Project.Tests
{
    public static class TestHelpers
    {
        public static VehicleMake CreateTestMake(int id = 1) => new()
        {
            Id = id,
            Name = $"Make{id}",
            Abbreviation = $"M{id}"
        };

        public static IMapper CreateTestMapper()
        {
            var config = new MapperConfiguration(cfg => 
                cfg.AddProfile<ServiceMappingProfile>());
            return config.CreateMapper();
        }
    }
}