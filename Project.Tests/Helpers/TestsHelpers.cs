using AutoMapper; // Critical addition
using Project.Service.Mappings;

namespace Project.Tests.Helpers
{
    public static class TestHelpers
    {
        public static IMapper CreateTestMapper()
        {
            var config = new MapperConfiguration(cfg => 
                cfg.AddProfile<ServiceMappingProfile>());
            return config.CreateMapper();
        }
    }
}