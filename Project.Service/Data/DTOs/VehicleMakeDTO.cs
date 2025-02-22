namespace Project.Service.Data.DTOs
{
    public class VehicleMakeDTO
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Abrv { get; set; } // Add missing property
    }
}