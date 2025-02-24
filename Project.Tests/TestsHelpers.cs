// Project.Tests/Helpers/TestHelpers.cs
using AutoMapper;

namespace Project.Tests.Helpers
{
    public static class TestHelpers
    {
        public static IMapper CreateTestMapper()
        {
            var config = new MapperConfiguration(cfg => 
                cfg.AddProfile<Project.Service.Mappings.ServiceMappingProfile>());
            return config.CreateMapper();
        }
    }
}