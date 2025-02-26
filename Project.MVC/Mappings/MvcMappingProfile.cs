// 1. Project.MVC/Mappings/MvcMappingProfile.cs (Updated)
using AutoMapper;
using Project.MVC.Helpers;
using Project.MVC.ViewModels;
using Project.Service.Data.DTOs;
using Project.Service.Data.Helpers;

namespace Project.MVC.Mappings
{
    public class MvcMappingProfile : Profile
    {
        public MvcMappingProfile()
        {
            // Vehicle mappings
            CreateMap<VehicleMakeDTO, VehicleMakeVM>().ReverseMap();
            CreateMap<VehicleModelDTO, VehicleModelVM>()
                .ForMember(dest => dest.MakeName, opt => opt.MapFrom(src => src.Make.Name))
                .ReverseMap();

            // Pagination conversion
            CreateMap<PaginatedList<VehicleMakeDTO>, PagedResult<VehicleMakeVM>>()
                .ConvertUsing<PaginatedListConverter<VehicleMakeDTO, VehicleMakeVM>>();
        }
    }

    public class PaginatedListConverter<TSource, TDestination> 
        : ITypeConverter<PaginatedList<TSource>, PagedResult<TDestination>>
    {
        public PagedResult<TDestination> Convert(
            PaginatedList<TSource> source, 
            PagedResult<TDestination> destination, 
            ResolutionContext context)
        {
            return new PagedResult<TDestination> {
                Items = context.Mapper.Map<List<TDestination>>(source.Items),
                TotalCount = source.TotalCount,
                PageNumber = source.PageIndex,
                PageSize = source.PageSize
            };
        }
    }
}