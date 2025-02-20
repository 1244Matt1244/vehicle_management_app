using AutoMapper;
using Project.Service.Data.DTOs;
using Project.Service.Data.Helpers;
using Project.MVC.ViewModels;

namespace Project.MVC.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // Individual Model Mappings
            CreateMap<VehicleModelDTO, VehicleModelVM>()
                .ReverseMap();
            
            CreateMap<VehicleMakeDTO, VehicleMakeVM>()
                .ReverseMap();

            // PaginatedList Mapping
            CreateMap<PaginatedList<VehicleMakeDTO>, PaginatedList<VehicleMakeVM>>()
                .ConvertUsing<PaginatedListConverter<VehicleMakeDTO, VehicleMakeVM>>();
            
            CreateMap<PaginatedList<VehicleModelDTO>, PaginatedList<VehicleModelVM>>()
                .ConvertUsing<PaginatedListConverter<VehicleModelDTO, VehicleModelVM>>();
        }
    }

    public class PaginatedListConverter<TSource, TDestination> : 
        ITypeConverter<PaginatedList<TSource>, PaginatedList<TDestination>>
    {
        public PaginatedList<TDestination> Convert(
            PaginatedList<TSource> source,
            PaginatedList<TDestination> destination,
            ResolutionContext context)
        {
            var mappedItems = context.Mapper.Map<List<TDestination>>(source.Items);
            
            return new PaginatedList<TDestination>(
                mappedItems,
                source.TotalCount,
                source.PageIndex,
                source.PageSize
            );
        }
    }
}