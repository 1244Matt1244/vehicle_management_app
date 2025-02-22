// Project.MVC/Mappings/MvcMappingProfile.cs
using AutoMapper;
using Project.Service.Data.DTOs;
using Project.MVC.ViewModels;

namespace Project.MVC.Mappings
{
    public class MvcMappingProfile : Profile
    {
        public MvcMappingProfile()
        {
            CreateMap<VehicleMakeDTO, VehicleMakeVM>().ReverseMap();
            CreateMap<PaginatedList<VehicleMakeDTO>, PaginatedList<VehicleMakeVM>>()
                .ConvertUsing((src, _, ctx) => new PaginatedList<VehicleMakeVM>(
                    ctx.Mapper.Map<List<VehicleMakeVM>>(src.Items),
                    src.TotalCount,
                    src.PageIndex,
                    src.PageSize
                ));
        }
    }
}