using System.Collections.Generic;
using Project.MVC.Helpers;

namespace Project.MVC.ViewModels
{
    public class VehicleModelIndexVM
    {
        public PagedResult<VehicleModelVM> PagedResults { get; set; } = new PagedResult<VehicleModelVM>();
        public List<VehicleMakeVM> Makes { get; set; } = new List<VehicleMakeVM>();

        // Add this property to hold the selected MakeId for filtering
        public int? MakeIdFilter { get; set; }
    }
}
