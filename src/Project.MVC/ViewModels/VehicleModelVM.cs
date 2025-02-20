using System.ComponentModel.DataAnnotations;

namespace Project.MVC.ViewModels
{
    public class VehicleModelVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(50, MinimumLength = 2)]
        public required string Name { get; set; }

        [StringLength(10)]
        public required string Abrv { get; set; }

        [Required(ErrorMessage = "Vehicle make is required")]
        public required int VehicleMakeId { get; set; } // Foreign key to VehicleMake

        // Optional: Add a reference to the related VehicleMakeVM
        public required VehicleMakeVM? VehicleMake { get; set; }
    }
}
