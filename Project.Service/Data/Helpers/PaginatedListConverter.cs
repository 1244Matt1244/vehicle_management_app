using AutoMapper;
using System.Collections.Generic;

namespace Project.Service.Data.Helpers
{
    public class PaginatedListConverter<TSource, TDestination> 
        : ITypeConverter<PaginatedList<TSource>, PaginatedList<TDestination>>
    {
        public PaginatedList<TDestination> Convert(
            PaginatedList<TSource> source,
            PaginatedList<TDestination> destination,
            ResolutionContext context)
        {
            // Map the list of items
            var mappedItems = context.Mapper.Map<List<TDestination>>(source.Items);

            // Reconstruct the PaginatedList<TDestination> using the parameterized constructor
            return new PaginatedList<TDestination>(
                mappedItems,
                source.TotalCount,
                source.PageIndex,
                source.PageSize
            );
        }
    }
}
