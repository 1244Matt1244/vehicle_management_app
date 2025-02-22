using AutoMapper;
using Project.Service.Data.DTOs;
using Project.Service.Data.Helpers;
using Project.MVC.ViewModels;
using System.Collections.Generic;

namespace Project.MVC.Mappings
{
    public class MvcMappingProfile : Profile
    {
        public MvcMappingProfile()
        {
            // VehicleMake mappings
            CreateMap<VehicleMakeDTO, VehicleMakeVM>().ReverseMap();
            CreateMap<PaginatedList<VehicleMakeDTO>, PaginatedList<VehicleMakeVM>>()
                .ConvertUsing((src, _, ctx) => new PaginatedList<VehicleMakeVM>(
                    ctx.Mapper.Map<List<VehicleMakeVM>>(src.Items),
                    src.TotalCount,
                    src.PageIndex,
                    src.PageSize
                ));

            // VehicleModel mappings
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