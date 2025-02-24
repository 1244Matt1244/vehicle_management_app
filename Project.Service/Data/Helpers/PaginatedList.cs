using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Service.Data.Helpers
{
    // PaginatedList for pagination in the application
    public class PaginatedList<T> : List<T>
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }
        
        // A read-only list of items on the current page
        public IReadOnlyList<T> Items => this;

        // Parameterless constructor for AutoMapper
        public PaginatedList() { }

        // Constructor to initialize the pagination parameters
        public PaginatedList(List<T> items, int totalCount, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalCount = totalCount;
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            AddRange(items);
        }

        // Determines if there is a previous page
        public bool HasPreviousPage => PageIndex > 1;

        // Determines if there is a next page
        public bool HasNextPage => PageIndex < TotalPages;

        // Static method to create a PaginatedList asynchronously
        public static async Task<PaginatedList<T>> CreateAsync(
            IQueryable<T> source, int pageIndex, int pageSize)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pageIndex - 1) * pageSize)
                                    .Take(pageSize)
                                    .ToListAsync();
            return new PaginatedList<T>(items, count, pageIndex, pageSize);
        }
    }
}
