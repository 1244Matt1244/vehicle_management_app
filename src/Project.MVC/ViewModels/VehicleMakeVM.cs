using System.ComponentModel.DataAnnotations;

namespace Project.MVC.ViewModels
{
    public class VehicleMakeVM
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Name is required")]
        [StringLength(50, MinimumLength = 2)]
        public required string Name { get; set; }
        
        [StringLength(10)]
        public required string Abrv { get; set; }
    }
}