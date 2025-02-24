using System.Collections.Generic;

namespace Project.Service.Models
{
    public class VehicleModel
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Abrv { get; set; }
        public required int MakeId { get; set; }          // Foreign key
    }
}