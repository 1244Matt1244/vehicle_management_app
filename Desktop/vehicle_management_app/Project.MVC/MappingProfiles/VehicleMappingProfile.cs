// Project.MVC/MappingProfiles/VehicleMappingProfile.cs
using AutoMapper;
using Project.Service.Models;
using Project.MVC.ViewModels;

namespace Project.MVC.MappingProfiles
{
    public class VehicleMappingProfile : Profile
    {
        public VehicleMappingProfile()
        {
            CreateMap<VehicleMake, VehicleMakeViewModel>().ReverseMap();
            CreateMap<VehicleModel, VehicleModelViewModel>().ReverseMap();
        }
    }
}
