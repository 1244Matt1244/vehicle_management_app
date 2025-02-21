using AutoMapper;
using Project.Service.Shared.Models;
using Project.Service.Shared.DTOs;

namespace Project.Service.Shared.Mapping
{
    public class ServiceMappingProfile : Profile
    {
        public ServiceMappingProfile()
        {
            CreateMap<VehicleMake, VehicleMakeDTO>()
                .ReverseMap();

            CreateMap<VehicleModel, VehicleModelDTO>()
                .ForMember(dest => dest.MakeName, opt => opt.MapFrom(src => src.Make.Name))
                .ReverseMap();
        }
    }
}
