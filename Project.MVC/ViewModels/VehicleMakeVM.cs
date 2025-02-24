// Project.MVC/ViewModels/VehicleMakeVM.cs
using Project.Service.Data.Helpers;

namespace Project.MVC.ViewModels
{
    public class VehicleMakeVM
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Abrv { get; set; } = string.Empty;
        public PaginatedList<VehicleMakeVM>? PaginatedList { get; set; }
        public string CurrentSort { get; set; } = string.Empty;
        public string SortOrder { get; set; } = string.Empty;
        public string SearchString { get; set; } = string.Empty;
    }
}