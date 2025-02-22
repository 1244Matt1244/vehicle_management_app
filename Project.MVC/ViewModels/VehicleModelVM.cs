namespace Project.MVC.ViewModels
{
    public class VehicleModelVM
    {
        public required int Id { get; set; }
        public required string Name { get; set; } 
        public required string Abrv { get; set; } 
        public required string MakeName { get; set; } 
        public required int MakeId { get; set; }
    }
}