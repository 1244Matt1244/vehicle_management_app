// Project.MVC/Mappings/MvcMappingProfile.cs
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
            CreateMap<VehicleModelDTO, VehicleModelVM>().ReverseMap();
        }
    }
}