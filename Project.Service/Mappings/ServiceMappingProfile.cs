using AutoMapper;
using Project.Service.Data.DTOs;
using Project.Service.Data.Helpers;
using Project.Service.Models;

namespace Project.Service.Mapping
{
    public class ServiceMappingProfile : Profile
    {
        public ServiceMappingProfile()
        {
            CreateMap<VehicleMake, VehicleMakeDTO>().ReverseMap();
            CreateMap<VehicleModel, VehicleModelDTO>().ReverseMap();
            
            CreateMap(typeof(PaginatedList<>), typeof(PaginatedList<>))
                .ConvertUsing(typeof(PaginatedListConverter<,>));
        }
    }
}