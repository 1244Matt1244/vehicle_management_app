using System.Collections.Generic;

namespace Project.Service.Models
{
    public class VehicleMake
    {

        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Abbreviation { get; set; }

        // Navigation property for related VehicleModels
        public ICollection<VehicleModel> VehicleModels { get; set; } = new List<VehicleModel>();
    }
}