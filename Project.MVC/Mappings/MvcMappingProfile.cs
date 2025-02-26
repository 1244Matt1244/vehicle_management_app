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
            // VehicleMake mappings
            CreateMap<VehicleMakeDTO, VehicleMakeVM>().ReverseMap();
            
            // VehicleModel mappings
            CreateMap<VehicleModelDTO, VehicleModelVM>()
                .ForMember(dest => dest.MakeName, opt => opt.MapFrom(src => src.MakeName))
                .ReverseMap();

            // Pagination type conversion
            CreateMap<PaginatedList<VehicleMakeDTO>, PagedResult<VehicleMakeVM>>()
                .ConvertUsing<PaginatedListConverter<VehicleMakeDTO, VehicleMakeVM>>();
            
            CreateMap<PaginatedList<VehicleModelDTO>, PagedResult<VehicleModelVM>>()
                .ConvertUsing<PaginatedListConverter<VehicleModelDTO, VehicleModelVM>>();
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
            return new PagedResult<TDestination>
            {
                Items = context.Mapper.Map<List<TDestination>>(source.Items),
                TotalCount = source.TotalCount,
                PageNumber = source.PageIndex,
                PageSize = source.PageSize
            };
        }
    }
}