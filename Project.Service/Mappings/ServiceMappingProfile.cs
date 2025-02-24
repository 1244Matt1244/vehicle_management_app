using AutoMapper;
using Project.Service.Data.Helpers;
using Project.Service.Data.DTOs;
using Project.Service.Models;

namespace Project.Service.Mappings
{
    public class ServiceMappingProfile : Profile
    {
        public ServiceMappingProfile()
        {
            // Map EF models to DTOs (two-way mapping)
            CreateMap<VehicleMake, VehicleMakeDTO>().ReverseMap();
            CreateMap<VehicleModel, VehicleModelDTO>().ReverseMap();

            // Map PaginatedList<VehicleMake> to PaginatedList<VehicleMakeDTO> using the custom converter
            CreateMap<PaginatedList<VehicleMake>, PaginatedList<VehicleMakeDTO>>()
                .ConvertUsing<PaginatedListConverter<VehicleMake, VehicleMakeDTO>>();
        }
    }
}
