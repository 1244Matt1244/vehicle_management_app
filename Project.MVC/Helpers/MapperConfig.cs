using AutoMapper;
using Project.MVC.Mappings; // Add this line

namespace Project.MVC.Helpers
{
    public static class MapperConfig
    {
        public static IMapper Initialize()
        {
            var config = new MapperConfiguration(cfg => {
                cfg.AddProfile<MvcMappingProfile>();
            });
            return config.CreateMapper();
        }
    }
}