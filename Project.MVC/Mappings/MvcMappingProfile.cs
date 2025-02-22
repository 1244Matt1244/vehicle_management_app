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
            // VehicleMakeDTO -> VehicleMakeVM and vice versa
            CreateMap<VehicleMakeDTO, VehicleMakeVM>().ReverseMap();
            
            // VehicleModelDTO -> VehicleModelVM and vice versa
            CreateMap<VehicleModelDTO, VehicleModelVM>().ReverseMap();

            // PaginatedList<VehicleMakeDTO> -> PaginatedList<VehicleMakeVM>
            CreateMap<PaginatedList<VehicleMakeDTO>, PaginatedList<VehicleMakeVM>>()
                .ConvertUsing((src, _, ctx) => new PaginatedList<VehicleMakeVM>(
                    ctx.Mapper.Map<List<VehicleMakeVM>>(src.Items),
                    src.TotalCount,
                    src.PageIndex,
                    src.PageSize
                ));

            // PaginatedList<VehicleModelDTO> -> PaginatedList<VehicleModelVM>
            CreateMap<PaginatedList<VehicleModelDTO>, PaginatedList<VehicleModelVM>>()
                .ConvertUsing((src, _, ctx) => new PaginatedList<VehicleModelVM>(
                    ctx.Mapper.Map<List<VehicleModelVM>>(src.Items),
                    src.TotalCount,
                    src.PageIndex,
                    src.PageSize
                ));
        }
    }
}
