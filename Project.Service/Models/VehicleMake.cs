using System.Collections.Generic;

namespace Project.Service.Models
{
    public class VehicleMake
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required string Abrv { get; set; }

    // Navigation property for VehicleModels
    public ICollection<VehicleModel> VehicleModels { get; set; } = new List<VehicleModel>();
}
}
