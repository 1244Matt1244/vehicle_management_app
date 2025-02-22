using System.Collections.Generic;

namespace Project.Service.Models
{
    public class VehicleModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Abrv { get; set; }
        public int MakeId { get; set; }          // Foreign key
        public VehicleMake VehicleMake { get; set; } // Navigation property
    }
}