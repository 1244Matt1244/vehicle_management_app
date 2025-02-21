// Project.Service/Utilities/PaginatedList.cs
using System.Collections.Generic;

namespace Project.Service.Data.Helpers
{
    public class PaginatedList<T>
    {
        public List<T> Items { get; set; }
        public int PageIndex { get; set; }
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }

        // Add constructor
        public PaginatedList(List<T> items, int pageIndex, int totalPages, int totalCount)
        {
            Items = items;
            PageIndex = pageIndex;
            TotalPages = totalPages;
            TotalCount = totalCount;
        }

        public bool HasPreviousPage => PageIndex > 1;
        public bool HasNextPage => PageIndex < TotalPages;
    }
}