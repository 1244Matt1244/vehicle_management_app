using AutoMapper;
using Project.Service.Data.DTOs;
using Project.MVC.ViewModels;

namespace Project.MVC.Mappings
{
    public class MvcMappingProfile : Profile
    {
        public MvcMappingProfile()
        {
            // Mapping from VehicleMakeDTO to VehicleMakeVM
            CreateMap<VehicleMakeDTO, VehicleMakeVM>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Abrv, opt => opt.MapFrom(src => src.Abrv)) // Include Abrv if applicable
                .ReverseMap(); // Allow reverse mapping

            // Additional mappings can be added here as needed
            // Example: CreateMap<VehicleModelDTO, VehicleModelVM>().ReverseMap();
        }
    }
}
