using Microsoft.AspNetCore.Mvc;
using Project.MVC.ViewModels;
using Project.Service.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Project.MVC.Controllers
{
    public class VehicleModelController : Controller
    {
        // Example action method
        public async Task<IActionResult> Index(string searchString)
        {
            var viewModel = new VehicleModelIndexVM
            {
                VehicleModels = new List<VehicleModelVM>(),
                SearchString = searchString
            };
            return View(viewModel);
        }
    }
}