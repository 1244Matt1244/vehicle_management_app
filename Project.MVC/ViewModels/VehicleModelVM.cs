namespace Project.MVC.ViewModels
{
    public class VehicleModelVM
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Abbreviation { get; set; }
        public required string MakeName { get; set; }
        public int MakeId { get; set; }
    }
}