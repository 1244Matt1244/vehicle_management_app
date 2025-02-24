using AutoMapper;
using Project.Service.Data.DTOs;
using Project.MVC.ViewModels;
using Project.Service.Data.Helpers;
using System.Collections.Generic;

namespace Project.MVC.Mappings
{
    public class MvcMappingProfile : Profile
    {
        public MvcMappingProfile()
        {
            // Map DTOs to ViewModels (and vice versa)
            CreateMap<VehicleMakeDTO, VehicleMakeVM>().ReverseMap();
            CreateMap<VehicleModelDTO, VehicleModelVM>().ReverseMap();

            // Map PaginatedList<VehicleMakeDTO> to PaginatedList<VehicleMakeVM>
            CreateMap<PaginatedList<VehicleMakeDTO>, PaginatedList<VehicleMakeVM>>()
                .ConvertUsing<PaginatedListConverter<VehicleMakeDTO, VehicleMakeVM>>();

            // Map PaginatedList<VehicleModelDTO> to PaginatedList<VehicleModelVM>
            CreateMap<PaginatedList<VehicleModelDTO>, PaginatedList<VehicleModelVM>>()
                .ConvertUsing<PaginatedListConverter<VehicleModelDTO, VehicleModelVM>>();
        }
    }
}
