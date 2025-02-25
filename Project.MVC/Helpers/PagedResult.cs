// Updated PagedResult.cs
using System;
using System.Collections.Generic;

namespace Project.MVC.Helpers
{
    public class PagedResult<T>
    {
        public List<T> Items { get; set; }  // Remove nullable marker
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);

        // Updated constructors
        public PagedResult(List<T> items, int totalCount, int pageIndex, int pageSize)
        {
            Items = items;
            TotalCount = totalCount;
            PageIndex = pageIndex;
            PageSize = pageSize;
        }

        // Initialize properties in parameterless constructor
        public PagedResult()
        {
            Items = new List<T>();
            PageIndex = 1;
            PageSize = 10;
            TotalCount = 0;
        }
    }
}