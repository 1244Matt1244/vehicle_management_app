using AutoMapper;
using Project.Service.Data.DTOs;
using Project.MVC.ViewModels;
using System.Collections.Generic;
using Project.Service.Data.Helpers;

namespace Project.MVC.Mappings
{
    public class MvcMappingProfile : Profile
    {
        public MvcMappingProfile()
        {
            // VehicleMake
            CreateMap<VehicleMakeDTO, VehicleMakeVM>().ReverseMap();
            CreateMap<PaginatedList<VehicleMakeDTO>, PaginatedList<VehicleMakeVM>>()
                .ConvertUsing((src, _, ctx) => new PaginatedList<VehicleMakeVM>(
                    ctx.Mapper.Map<List<VehicleMakeVM>>(src.Items),
                    src.TotalCount,
                    src.PageIndex,
                    src.PageSize
                ));

            // VehicleModel
            CreateMap<VehicleModelDTO, VehicleModelVM>().ReverseMap();
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