using AutoMapper;
using Project.Service.Data.DTOs;
using Project.Service.Data.Helpers; // Add this
using Project.Service.Models;

namespace Project.Service.Mapping
{
    public class ServiceMappingProfile : Profile
    {
        public ServiceMappingProfile()
        {
            // VehicleMake Mappings
            CreateMap<VehicleMake, VehicleMakeDTO>().ReverseMap();

            // VehicleModel Mappings
            CreateMap<VehicleModel, VehicleModelDTO>()
                .ForMember(dest => dest.MakeName, opt => opt.MapFrom(src => src.Make.Name))
                .ReverseMap();

            // PaginatedList Mappings
            CreateMap<PaginatedList<VehicleMake>, PaginatedList<VehicleMakeDTO>>();
            CreateMap<PaginatedList<VehicleModel>, PaginatedList<VehicleModelDTO>>();
        }
    }
}