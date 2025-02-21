using System.Collections.Generic;

namespace Project.Data.Models
{
    public class VehicleModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int MakeId { get; set; }
        public VehicleMake Make { get; set; }
    }
}