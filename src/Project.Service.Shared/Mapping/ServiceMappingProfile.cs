// In ServiceMappings.cs
using AutoMapper;
using Project.Service.Shared.Models; // Add missing namespace
using Project.Service.Shared.DTOs;

public class ServiceMappings : Profile
{
    public ServiceMappings()
    {
        CreateMap<VehicleMake, VehicleMakeDTO>()
            .ReverseMap();

        CreateMap<VehicleModel, VehicleModelDTO>()
            .ForMember(dest => dest.MakeName, opt => opt.MapFrom(src => src.Make.Name))
            .ReverseMap();
    }
}
