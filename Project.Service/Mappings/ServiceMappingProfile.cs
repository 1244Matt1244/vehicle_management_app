using AutoMapper;
using Project.Service.Data.DTOs;
using Project.Service.Models;

namespace Project.Service.Mappings
{
    public class ServiceMappingProfile : Profile
    {
        public ServiceMappingProfile()
        {
            // VehicleMake
            CreateMap<VehicleMake, VehicleMakeDTO>().ReverseMap();

            // VehicleModel
            CreateMap<VehicleModel, VehicleModelDTO>()
                .ForMember(dest => dest.MakeName, opt => opt.MapFrom(src => src.VehicleMake.Name))
                .ReverseMap();
        }
    }
}