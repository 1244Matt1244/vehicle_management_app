// Project.MVC/ViewModels/VehicleMakeIndexVM.cs
using System.Collections.Generic;

namespace Project.MVC.ViewModels
{
    public class VehicleMakeIndexVM
    {
        public List<VehicleMakeVM> Items { get; set; } = new List<VehicleMakeVM>();
        public PaginationVM Pagination { get; set; }
        public string CurrentSort { get; set; }
        public string SortOrder { get; set; }
        public string SearchString { get; set; }
    }
}