// Project.Service/VehicleModule.cs
using Project.Service.Mappings; 
using Microsoft.Extensions.DependencyInjection;

namespace Project.Service
{
    public static class VehicleModule
    {
        public static IServiceCollection AddVehicleServices(this IServiceCollection services)
        {
            // Configuration
            return services;
        }
    }
}