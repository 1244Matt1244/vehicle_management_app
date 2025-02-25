using AutoMapper;
using Project.MVC.ViewModels;
using Project.Service.Data.DTOs;

namespace Project.MVC.Mappings
{
    public class MvcMappingProfile : Profile
    {
        public MvcMappingProfile()
        {
            // VehicleMake mappings
            CreateMap<VehicleMakeDTO, VehicleMakeVM>()
                .ReverseMap();

            // VehicleModel mappings
            CreateMap<VehicleModelDTO, VehicleModelVM>()
                .ForMember(dest => dest.MakeName, opt => opt.MapFrom(src => src.MakeName))
                .ReverseMap();
        }
    }
}