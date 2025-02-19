using AutoMapper;
using Project.Service.DTOs;
using Project.Service.Models;

namespace Project.Service.Mapping
{
    public class ServiceMappingProfile : Profile
    {
        public ServiceMappingProfile()
        {
            CreateMap<VehicleMake, VehicleMakeDTO>().ReverseMap();
            CreateMap<VehicleModel, VehicleModelDTO>()
                .ForMember(dest => dest.MakeName, opt => opt.MapFrom(src => src.VehicleMake.Name))
                .ReverseMap();
        }
    }
}
