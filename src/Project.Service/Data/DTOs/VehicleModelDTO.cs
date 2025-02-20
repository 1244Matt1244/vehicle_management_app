using System.ComponentModel.DataAnnotations;

namespace Project.Service.Data.DTOs
{

    public class VehicleModelDTO
    {
        public required int Id { get; set; }
        public required string Name { get; set; } = string.Empty;
        public required string Abrv { get; set; } = string.Empty;
        public required int MakeName { get; set; }
    }
}
