// VehicleModelCreateVM.cs
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Project.MVC.ViewModels;

namespace Project.MVC.ViewModels
{
    public class VehicleModelCreateVM
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Abbreviation is required")]
        public string Abbreviation { get; set; } = string.Empty;

        [Required(ErrorMessage = "Make selection is required")]
        public int MakeId { get; set; }
        
        public List<VehicleMakeVM>? AvailableMakes { get; set; }
    }
}