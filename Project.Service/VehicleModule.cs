// Project.Service/VehicleModule.cs
using Microsoft.Extensions.DependencyInjection;
using AutoMapper;
using Project.Service.Mappings; // Correct namespace

namespace Project.Service
{
    public static class VehicleModule
    {
        public static IServiceCollection AddVehicleServices(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(ServiceMappingProfile));
            return services;
        }
    }
}