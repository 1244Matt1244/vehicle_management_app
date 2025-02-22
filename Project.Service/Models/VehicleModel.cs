using System.Collections.Generic;

namespace Project.Service.Models
{
    public class VehicleModel
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required string Abrv { get; set; }
        public required int MakeId { get; set; }          // Foreign key
        public VehicleMake? VehicleMake { get; set; } // Navigation property (nullable)
    }
}