using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Service.Models
{
    public class VehicleModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [StringLength(10)]
        public string Abbreviation { get; set; }

        [ForeignKey("VehicleMake")]
        public int MakeId { get; set; }

        public virtual VehicleMake VehicleMake { get; set; }  // Correct navigation property name
    }
}