using AutoMapper;
using Project.Service.Data.DTOs;
using Project.MVC.ViewModels;

namespace Project.MVC.Mappings
{
    public class MvcMappingProfile : Profile
    {
        public MvcMappingProfile()
        {
            // VehicleMake
            CreateMap<VehicleMakeDTO, VehicleMakeVM>()
                .ForMember(dest => dest.PaginatedList, opt => opt.Ignore()) // Handled separately
                .ReverseMap();

            // VehicleModel
            CreateMap<VehicleModelDTO, VehicleModelVM>()
                .ForMember(dest => dest.PaginatedList, opt => opt.Ignore())
                .ReverseMap();

            // PaginatedList mappings
            CreateMap<PaginatedList<VehicleMakeDTO>, PaginatedList<VehicleMakeVM>>()
                .ConvertUsing((src, _, ctx) => new PaginatedList<VehicleMakeVM>(
                    ctx.Mapper.Map<List<VehicleMakeVM>>(src.Items),
                    src.TotalCount,
                    src.PageIndex,
                    src.PageSize
                ));

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