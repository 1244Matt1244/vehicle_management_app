// Project.MVC/ViewModels/VehicleModelIndexVM.cs
using System.Collections.Generic;

namespace Project.MVC.ViewModels
{
    public class VehicleModelIndexVM
    {
        public List<VehicleModelVM> Items { get; set; } = new List<VehicleModelVM>();
        public PaginationVM Pagination { get; set; }
        public List<VehicleMakeVM> Makes { get; set; } // For dropdown
        public int CurrentMakeId { get; set; }
        public string CurrentSort { get; set; }
        public string SortOrder { get; set; }
        public string SearchString { get; set; }
    }
}