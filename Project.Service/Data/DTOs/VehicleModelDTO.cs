namespace Project.Service.Data.DTOs
{
    public class VehicleModelDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Abbreviation { get; set; }
        public string MakeName { get; set; }  // Renamed from Make to MakeName
    }
}