using AutoMapper;
using Project.Service.DTOs;
using Project.Service.Models;

namespace Project.Service.Mapping
{
    public class ServiceMappingProfile : Profile
    {
        public ServiceMappingProfile()
        {
            // Mapping for VehicleMake <-> VehicleMakeDTO
            CreateMap<VehicleMake, VehicleMakeDTO>()
                .ReverseMap();

            // Mapping for VehicleModel <-> VehicleModelDTO
            CreateMap<VehicleModel, VehicleModelDTO>()
                .ForMember(dest => dest.MakeName, opt => opt.MapFrom(src => src.VehicleMake.Name)) // Map MakeName from VehicleMake.Name
                .ReverseMap()
                .ForMember(dest => dest.VehicleMake, opt => opt.Ignore()); // Ignore VehicleMake to avoid circular references
        }
    }
}
