using Microsoft.Extensions.DependencyInjection;
using AutoMapper;

namespace Project.Service
{
    public static class VehicleModule
    {
        public static IServiceCollection AddVehicleServices(this IServiceCollection services)
        {
            // AutoMapper Configuration
            services.AddAutoMapper(typeof(Mappings.ServiceMappingProfile));
            return services;
        }
    }
}