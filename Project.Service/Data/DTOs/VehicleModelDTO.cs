// In Project.Service/DTOs/VehicleModelDTO.cs
public class VehicleModelDTO
{
    public required int Id { get; set; }
    public required string Name { get; set; } = string.Empty;
    public required string Abrv { get; set; } = string.Empty;
    // Change to match entity relationship
    public required int MakeId { get; set; }
    public required string MakeName { get; set; } // If you want to include Make name
}
