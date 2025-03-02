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
        public required string Name { get; set; }

        [Required]
        [StringLength(10)]
        public required string Abbreviation { get; set; }

        [ForeignKey(nameof(VehicleMake))]  // Explicit foreign key annotation
        public int VehicleMakeId { get; set; }  // Correct property name

        [Required]
        public virtual VehicleMake VehicleMake { get; set; } = null!;
    }
}