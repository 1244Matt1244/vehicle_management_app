using System.ComponentModel.DataAnnotations;

namespace Project.MVC.ViewModels
{
    public class VehicleMakeVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(50, ErrorMessage = "Name cannot exceed 50 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Abbreviation is required.")]
        [StringLength(10, ErrorMessage = "Abbreviation cannot exceed 10 characters.")]
        public string Abbreviation { get; set; }
    }
}