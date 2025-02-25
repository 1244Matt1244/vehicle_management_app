// Project.Service/Data/DTOs/VehicleModelDTO.cs
namespace Project.Service.Data.DTOs
{
    public class VehicleModelDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Abbreviation { get; set; } = string.Empty;
        public int MakeId { get; set; }
        public string MakeName { get; set; } = string.Empty; 
    }
}