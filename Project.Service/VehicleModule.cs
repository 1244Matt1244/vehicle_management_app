using Microsoft.Extensions.DependencyInjection;
using Project.Service.Mappings;
using AutoMapper;

namespace Project.Service
{
    public static class VehicleModule
    {
        public static IServiceCollection AddVehicleServices(this IServiceCollection services)
        {
            // AutoMapper Configuration
            services.AddAutoMapper(typeof(ServiceMappingProfile));

            // Add other service configurations here
            
            return services;
        }
    }
}