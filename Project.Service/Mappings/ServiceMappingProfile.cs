// ServiceMappingProfile.cs
using AutoMapper;
using Project.Service.Data.Helpers;
using Project.Service.Models;
using Project.Service.Data.DTOs;

namespace Project.Service.Mappings
{
    public class ServiceMappingProfile : Profile
    {
        public ServiceMappingProfile()
        {
            // VehicleMake ↔ VehicleMakeDTO
            CreateMap<VehicleMake, VehicleMakeDTO>().ReverseMap();
            
            // VehicleModel ↔ VehicleModelDTO
            CreateMap<VehicleModel, VehicleModelDTO>().ReverseMap();

            // Generic PaginatedList mapping
            CreateMap(typeof(PaginatedList<>), typeof(PaginatedList<>))
                .ConvertUsing(typeof(PaginatedListConverter<,>));
        }
    }
}