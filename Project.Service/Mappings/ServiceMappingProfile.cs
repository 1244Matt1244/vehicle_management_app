using AutoMapper;
using Project.Service.Data.Helpers;
using Project.Service.Data.DTOs;
using Project.Service.Models;

namespace Project.Service.Mappings
{
    public class ServiceMappingProfile : Profile
    {
        public ServiceMappingProfile()
        {
            // VehicleMake ↔ VehicleMakeDTO
            CreateMap<VehicleMake, VehicleMakeDTO>().ReverseMap();
            CreateMap<VehicleModel, VehicleModelDTO>().ReverseMap();

            // Configure PaginatedList mapping
            CreateMap(typeof(PaginatedList<>), typeof(PaginatedList<>))
                .ConvertUsing(typeof(PaginatedListConverter<,>));
        }
    }
}