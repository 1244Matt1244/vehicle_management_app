using AutoMapper;
using Project.Service.Data.DTOs;
using Project.Service.Models;

namespace Project.Service.Mappings
{
    public class ServiceMappingProfile : Profile
    {
        public ServiceMappingProfile()
        {
            // VehicleMake mappings
            CreateMap<VehicleMake, VehicleMakeDTO>()
                .ReverseMap();

            // VehicleModel mappings with correct navigation property
            CreateMap<VehicleModel, VehicleModelDTO>()
                .ForMember(dest => dest.MakeName, 
                          opt => opt.MapFrom(src => src.VehicleMake.Name))  // Fixed to use VehicleMake
                .ReverseMap()
                .ForPath(src => src.VehicleMake.Name, 
                        opt => opt.Ignore());  // Prevent overwrite of navigation property
        }
    }
}