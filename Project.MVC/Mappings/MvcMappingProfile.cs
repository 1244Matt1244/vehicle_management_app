using AutoMapper;
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
                .ForMember(dest => dest.MakeName, opt => opt.MapFrom(src => src.Make.Name)) // Fix mapping
                .ReverseMap();

            // Pagination type conversion (Resolve PaginatedList -> PagedResult)
            CreateMap<PaginatedList<VehicleMakeDTO>, PagedResult<VehicleMakeVM>>()
                .ConvertUsing<PaginatedListConverter<VehicleMakeDTO, VehicleMakeVM>>();
            
            CreateMap<PaginatedList<VehicleModelDTO>, PagedResult<VehicleModelVM>>()
                .ConvertUsing<PaginatedListConverter<VehicleModelDTO, VehicleModelVM>>();
        }
    }

    // Define the converter within the Mappings namespace
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
                PageNumber = source.PageIndex, // Align with your PagedResult properties
                PageSize = source.PageSize
            };
        }
    }
}