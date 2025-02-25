using System.Collections.Generic;
using Project.MVC.Helpers;

namespace Project.MVC.ViewModels
{
    public class VehicleMakeIndexVM
    {
        public List<VehicleMakeVM> Items { get; set; } = new List<VehicleMakeVM>();
        public PaginationVM Pagination { get; set; } = new PaginationVM();
        public string CurrentSort { get; set; } = string.Empty;
        public string SortOrder { get; set; } = string.Empty;
        public string SearchString { get; set; } = string.Empty;
    }
}