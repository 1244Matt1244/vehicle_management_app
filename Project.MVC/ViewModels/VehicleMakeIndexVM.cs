// Project.MVC/ViewModels/VehicleMakeIndexVM.cs
using System.Collections.Generic;

namespace Project.MVC.ViewModels
{
    public class VehicleMakeIndexVM
    {
        public List<VehicleMakeVM> Items { get; set; } = new();
        public PaginationVM Pagination { get; set; } = new();
        public string CurrentSort { get; set; } = string.Empty;
        public string SortOrder { get; set; } = string.Empty; // Added property
        public string SearchString { get; set; } = string.Empty;
    }
}