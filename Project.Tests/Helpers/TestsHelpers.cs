using AutoMapper;
using Project.MVC.Mappings;
using Project.Service.Mappings;

namespace Project.Tests
{
    public static class TestHelpers
    {
        public static IMapper CreateTestMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                // Load both mapping profiles
                cfg.AddProfile(new ServiceMappingProfile());
                cfg.AddProfile(new MvcMappingProfile());
            });

            config.AssertConfigurationIsValid();
            return config.CreateMapper();
        }
    }
}