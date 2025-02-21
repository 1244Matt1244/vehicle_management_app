using System.Collections.Generic;

namespace Project.Service.Shared.Helpers
{
    public class PaginatedList<T> : List<T>
    {
        public int PageIndex { get; }
        public int TotalPages { get; }
        public int TotalCount { get; }
        public bool HasPreviousPage => PageIndex > 1;
        public bool HasNextPage => PageIndex < TotalPages;

        public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            TotalPages = (int)System.Math.Ceiling(count / (double)pageSize);
            TotalCount = count;
            AddRange(items);
        }
    }
}