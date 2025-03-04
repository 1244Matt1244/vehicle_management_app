using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Project.Service.Models
{
    public class VehicleMake
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public required string Name { get; set; }

        [Required]
        [StringLength(10)]
        public required string Abbreviation { get; set; }

        public ICollection<VehicleModel> VehicleModels { get; set; } = new List<VehicleModel>();
    }
}