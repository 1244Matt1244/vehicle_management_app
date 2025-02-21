using System.Collections.Generic;

namespace Project.Service.Models
{
    public class VehicleMake
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<VehicleModel> Models { get; set; }
    }
}
