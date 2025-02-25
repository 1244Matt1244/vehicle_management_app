using System.Collections.Generic;
using Project.MVC.Helpers;

namespace Project.MVC.ViewModels
{
    public class VehicleModelIndexVM
    {
        public PagedResult<VehicleModelVM> PagedResults { get; set; } = new PagedResult<VehicleModelVM>();
        public List<VehicleMakeVM> Makes { get; set; } = new List<VehicleMakeVM>();
    }
}