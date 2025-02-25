// Project.MVC/Mapping/ServiceMappingProfile.cs
using AutoMapper;
using Project.MVC.ViewModels;
using Project.Service.Data.DTOs;

public class ServiceMappingProfile : Profile
{
    public ServiceMappingProfile()
    {
        // VehicleMake
        CreateMap<VehicleMakeDTO, VehicleMakeVM>().ReverseMap();
        
        // VehicleModel
        CreateMap<VehicleModelDTO, VehicleModelVM>().ReverseMap();
    }
}