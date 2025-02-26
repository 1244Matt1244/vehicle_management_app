namespace Project.MVC.ViewModels
{
    public class VehicleMakeVM
    {
        public int Id { get; set; }
        public required string Name { get; set; } // Added 'required'
        public required string Abbreviation { get; set; } // Added 'required'
    }
}