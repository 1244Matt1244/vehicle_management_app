using AutoMapper;
using Project.MVC.ViewModels;
using Project.Service.Data.DTOs;

namespace Project.MVC.Mappings
{
    public class MvcMappingProfile : Profile
    {
        public MvcMappingProfile()
        {
            // VehicleModel mapping
            CreateMap<VehicleModelDTO, VehicleModelVM>()
                .ForMember(dest => dest.MakeId, opt => opt.MapFrom(src => src.MakeId))  // Correct mapping
                .ReverseMap();
        }
    }
}