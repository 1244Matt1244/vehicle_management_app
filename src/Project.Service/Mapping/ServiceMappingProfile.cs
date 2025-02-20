using AutoMapper;
using Project.Service.Data.DTOs;
using Project.Service.Models;

namespace Project.Service.Mapping
{
    public class ServiceMappingProfile : Profile
    {
        public ServiceMappingProfile()
        {
            // Mapping for VehicleMake <-> VehicleMakeDTO
            CreateMap<VehicleMake, VehicleMakeDTO>()
                .ReverseMap();
        }
    }
}
