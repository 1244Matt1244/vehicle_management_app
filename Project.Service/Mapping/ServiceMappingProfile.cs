// Project.Service/Mapping/ServiceMappingProfile.cs
using AutoMapper;
using Project.Service.Data.DTOs;
using Project.Service.Models;

namespace Project.Service.Mapping
{
    public class ServiceMappingProfile : Profile
    {
        public ServiceMappingProfile()
        {
            CreateMap<VehicleMake, VehicleMakeDTO>().ReverseMap();
            CreateMap<VehicleModel, VehicleModelDTO>()
                .ForMember(dest => dest.MakeName, opt => opt.MapFrom(src => src.Make.Name))
                .ReverseMap()
                .ForMember(dest => dest.Make, opt => opt.Ignore()); // Avoid circular reference
        }
    }
}
