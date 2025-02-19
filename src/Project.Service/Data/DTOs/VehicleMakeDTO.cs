using System.ComponentModel.DataAnnotations;

namespace Project.Service.DTOs
{
    public class VehicleMakeDTO
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [StringLength(10)]
        public string Abrv { get; set; }
    }
}
