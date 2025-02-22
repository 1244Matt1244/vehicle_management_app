using AutoMapper;
using Project.Service.Data.DTOs;
using Project.Service.Models;

namespace Project.Service.Mappings
{
    public class ServiceMappingProfile : Profile
    {
        public ServiceMappingProfile()
        {
            // VehicleMake -> VehicleMakeDTO and vice versa
            CreateMap<VehicleMake, VehicleMakeDTO>().ReverseMap();
            
            // VehicleModel -> VehicleModelDTO and vice versa
            CreateMap<VehicleModel, VehicleModelDTO>().ReverseMap();
        }
    }
}
