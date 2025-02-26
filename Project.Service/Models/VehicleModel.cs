using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Service.Models
{
    public class VehicleModel
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Abbreviation { get; set; }
        
        [ForeignKey("VehicleMake")]
        public int MakeId { get; set; }
        public required VehicleMake VehicleMake { get; set; }
    }
}