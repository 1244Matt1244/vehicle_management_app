using System.ComponentModel.DataAnnotations;

namespace Project.Service.DTOs
{
    public class VehicleModelDTO
    {
        public int Id { get; set; }
        public int MakeId { get; set; }

        [Required]
        [StringLength(50)]
        public required string Name { get; set; }

        [Required]
        [StringLength(10)]
        public required string Abrv { get; set; }

        public required string MakeName { get; set; }
    }
}
