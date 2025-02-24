namespace Project.Service.Data.DTOs
{
    public class VehicleModelDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Abrv { get; set; } = string.Empty;
        public int MakeId { get; set; }
    }
}