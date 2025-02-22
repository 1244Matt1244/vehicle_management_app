// MvcMappingProfile.cs
using AutoMapper;
using Project.Service.Data.DTOs;
using Project.MVC.ViewModels;

namespace Project.MVC.Mappings
{
    public class MvcMappingProfile : Profile
    {
        public MvcMappingProfile()
        {
            // VehicleMakeDTO ↔ VehicleMakeVM
            CreateMap<VehicleMakeDTO, VehicleMakeVM>().ReverseMap();
            
            // VehicleModelDTO ↔ VehicleModelVM
            CreateMap<VehicleModelDTO, VehicleModelVM>().ReverseMap();

            // No explicit PaginatedList mapping needed - handled by ServiceMappingProfile
        }
    }
}