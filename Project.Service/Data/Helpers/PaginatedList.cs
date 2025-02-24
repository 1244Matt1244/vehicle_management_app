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
        // Properties to store pagination info
        public int PageIndex { get; }
        public int PageSize { get; }
        public int TotalPages { get; }
        public int TotalCount { get; }
        
        // A read-only list of items on the current page
        public IReadOnlyList<T> Items => this;

        // Constructor to initialize the pagination parameters
        public PaginatedList(List<T> items, int totalCount, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalCount = totalCount;
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);  // Calculate total pages
            AddRange(items);  // Add items to the list
        }

        // Determines if there is a previous page
        public bool HasPreviousPage => PageIndex > 1;

        // Determines if there is a next page
        public bool HasNextPage => PageIndex < TotalPages;

        // Static method to create a PaginatedList asynchronously
        public static async Task<PaginatedList<T>> CreateAsync(
            IQueryable<T> source, int pageIndex, int pageSize)
        {
            // Get the total number of records in the source
            var count = await source.CountAsync();

            // Fetch the items for the current page
            var items = await source.Skip((pageIndex - 1) * pageSize)
                                    .Take(pageSize)
                                    .ToListAsync();

            // Return a new PaginatedList with the items and pagination info
            return new PaginatedList<T>(items, count, pageIndex, pageSize);
        }
    }
}
