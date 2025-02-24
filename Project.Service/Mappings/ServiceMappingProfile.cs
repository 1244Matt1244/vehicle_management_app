// In Project.Service/Profiles/ServiceMappingProfile.cs
using AutoMapper;
using Project.MVC.ViewModels;
using Project.Service.Models;

namespace Project.Service.Mappings
{
    public class ServiceMappingProfile : Profile
    {
        public ServiceMappingProfile()
        {
            CreateMap<VehicleMake, VehicleMakeVM>()
                .ForMember(dest => dest.Abbreviation, opt => opt.MapFrom(src => src.Abrv))
                .ReverseMap();
        }
    }
}