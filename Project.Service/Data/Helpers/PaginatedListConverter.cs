using AutoMapper;
using Project.Service.Data;
using System.Collections.Generic;

namespace Project.Service.Mappings
{
    public class PaginatedListConverter<TSource, TDestination> 
        : ITypeConverter<PaginatedList<TSource>, PaginatedList<TDestination>>
    {
        public PaginatedList<TDestination> Convert(
            PaginatedList<TSource> source,
            PaginatedList<TDestination> destination,
            ResolutionContext context)
        {
            var mappedItems = context.Mapper.Map<List<TDestination>>(source);
            return new PaginatedList<TDestination>(
                mappedItems,
                source.TotalCount,
                source.PageIndex,
                source.PageSize);
        }
    }
}