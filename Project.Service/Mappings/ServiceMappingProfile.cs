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
            // Explicit mapping from VehicleMake to VehicleMakeDTO
            CreateMap<VehicleMake, VehicleMakeDTO>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Abrv, opt => opt.MapFrom(src => src.Abrv)) // Add this line if Abrv exists
                .ReverseMap(); // Allow reverse mapping

            // Register the PaginatedListConverter
            CreateMap<PaginatedList<VehicleMake>, PaginatedList<VehicleMakeDTO>>()
                .ConvertUsing<PaginatedListConverter<VehicleMake, VehicleMakeDTO>>();

            // Add similar mappings for VehicleModel if needed
            // CreateMap<VehicleModel, VehicleModelDTO>().ReverseMap(); // Example
        }
    }
}
