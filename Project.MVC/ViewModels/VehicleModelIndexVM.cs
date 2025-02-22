using System.Collections.Generic;

namespace Project.MVC.ViewModels
{
    public class VehicleModelIndexVM
    {
        public List<VehicleModelVM> Items { get; set; } = new List<VehicleModelVM>();
        public PaginationVM Pagination { get; set; } = new PaginationVM();
        public List<VehicleMakeVM> Makes { get; set; } = new List<VehicleMakeVM>();
        public int? CurrentMakeId { get; set; }
        public string CurrentSort { get; set; } = string.Empty;
        public string SearchString { get; set; } = string.Empty;
    }
}