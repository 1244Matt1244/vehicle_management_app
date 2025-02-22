namespace Project.MVC.ViewModels
{
    public class VehicleModelVM
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Abrv { get; set; } = string.Empty; // Added Abrv
        public string MakeName { get; set; } = string.Empty;
        public int MakeId { get; set; }
    }
}