using AutoMapper;
using Project.Service.Data.Helpers;
using Project.Service.Data.DTOs;
using Project.Service.Mappings;
using Project.Service.Models;

namespace Project.Service.Mappings
{

    public class ServiceMappingProfile : Profile
    {
    public ServiceMappingProfile()
    {
        // Register the PaginatedListConverter
        CreateMap<PaginatedList<VehicleMake>, PaginatedList<VehicleMakeDTO>>()
            .ConvertUsing<PaginatedListConverter<VehicleMake, VehicleMakeDTO>>();
    }
    } 
} 
