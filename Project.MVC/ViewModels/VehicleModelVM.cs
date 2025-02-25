namespace Project.MVC.ViewModels
{
    public class VehicleModelVM
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Abbreviation { get; set; } = string.Empty;  // Correct property name
        public string MakeName { get; set; } = string.Empty;
    }
}