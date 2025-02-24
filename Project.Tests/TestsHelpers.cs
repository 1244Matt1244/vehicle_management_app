// In Project.Tests/Helpers/TestHelpers.cs
using Project.MVC.ViewModels;
using Project.Service.Models;
using Microsoft.AspNetCore.Mvc;  // For ViewResult
using Project.Service.Mappings;  // For ServiceMappingProfile
using Project.Service.Data.Helpers;    // For PaginatedList<>

namespace Project.Tests.Helpers
{
    public static class TestHelpers
    {
        public static List<VehicleMake> GetTestVehicleMakes()
        {
            return new List<VehicleMake>
            {
                new VehicleMake { Id = 1, Name = "Make1", Abrv = "M1" },
                new VehicleMake { Id = 2, Name = "Make2", Abrv = "M2" }
            };
        }

        public static IMapper CreateTestMapper()
        {
            var configuration = new MapperConfiguration(cfg => 
                cfg.AddProfile<Project.Service.Mappings.ServiceMappingProfile>());
            return configuration.CreateMapper();
        }
    }
}