// Project.Service.Shared/Mappings/ServiceMappings.cs
using AutoMapper;
using Project.Service.Shared.DTOs;
using Project.Service.Models;

public class ServiceMappings : Profile
{
    public ServiceMappings()
    {
        // EF Model <-> DTO conversions
        CreateMap<VehicleMake, VehicleMakeDTO>()
            .ForMember(dest => dest.Models, opt => opt.Ignore())
            .ReverseMap();
        
        CreateMap<VehicleModel, VehicleModelDTO>()
            .ForMember(dest => dest.Make, opt => opt.MapFrom(src => src.Make))
            .ReverseMap();
    }
}
