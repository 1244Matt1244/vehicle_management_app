using System.Collections.Generic;

namespace Project.Service.Models
{
    public class VehicleMake
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required string Abrv { get; set; }

        public required ICollection<VehicleModel> VehicleModels { get; set; }
    }
}
