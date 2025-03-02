using System.Collections.Generic;

namespace Project.MVC.ViewModels
{
    public class VehicleModelEditVM
    {
        public int Id { get; set; }
        public required string Name { get; set; } = string.Empty;
        public required string Abbreviation { get; set; } = string.Empty;
        public int MakeId { get; set; }
        public List<VehicleMakeVM> AvailableMakes { get; set; } = new List<VehicleMakeVM>();

        public VehicleModelEditVM() {} // Default constructor
    }
}
