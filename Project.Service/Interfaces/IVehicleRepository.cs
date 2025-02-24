// Project.Service/Interfaces/IVehicleRepository.cs
using Project.Service.Data.Helpers;
using Project.Service.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Project.Service.Interfaces
{
    public interface IVehicleRepository
    {
        // Add these methods
        Task<List<VehicleMake>> GetAllMakesAsync();
        Task<List<VehicleModel>> GetAllModelsAsync();
    }
}