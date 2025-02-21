namespace Project.Service.Data.DTOs
{
    public class VehicleModelDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Abrv { get; set; } = string.Empty;
        public int VehicleMakeId { get; set; }
        public string MakeName { get; set; } = string.Empty;
    }
}
