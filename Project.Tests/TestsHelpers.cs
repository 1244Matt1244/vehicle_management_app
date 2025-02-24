using AutoMapper;
using Project.Service.Mappings;
using Project.MVC.Mappings;

namespace Project.Tests
{
    public static class Helpers
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
    }
}