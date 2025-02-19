namespace Project.Service.Data.DTOs
{
    public class VehicleModelDTO
    {
        public int Id { get; set; }
        public int MakeId { get; set; }
        public string MakeName { get; set; } // For display purposes
        public string Name { get; set; }
        public string Abrv { get; set; }
    }
}