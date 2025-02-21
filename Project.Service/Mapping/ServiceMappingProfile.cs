using AutoMapper;
using Project.Data.Models;
using Project.Service.Data.DTOs;

namespace Project.Service.Mappings
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
        }
    }
}
