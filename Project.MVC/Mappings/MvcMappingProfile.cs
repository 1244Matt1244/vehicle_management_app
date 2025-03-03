using AutoMapper;
using Project.Service.Data.DTOs;
using Project.MVC.ViewModels;

namespace Project.MVC.Mappings
{
    public class MvcMappingProfile : Profile
    {
        public MvcMappingProfile()
        {
            CreateMap<VehicleMakeDTO, VehicleMakeVM>().ReverseMap();
            CreateMap<VehicleModelDTO, VehicleModelVM>()
                .ForMember(dest => dest.MakeName, opt => opt.MapFrom(src => src.MakeName))
                .ReverseMap();
        }
    }
}
