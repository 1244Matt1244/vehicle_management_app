using Project.Service.Interfaces;
using Project.MVC.ViewModels;

namespace Project.Service.Services
{
    public class VehicleService : IVehicleService
    {
        // Mock data for demonstration purposes
        private readonly List<VehicleMakeVM> _makes = new();
        private readonly List<VehicleModelVM> _models = new();

        public Task<IEnumerable<VehicleModelVM>> GetModelsAsync()
        {
            return Task.FromResult(_models.AsEnumerable());
        }

        public Task CreateModelAsync(VehicleModelVM model)
        {
            model.Id = _models.Count + 1;
            _models.Add(model);
            return Task.CompletedTask;
        }

        public Task<VehicleModelVM> GetModelByIdAsync(int id)
        {
            var model = _models.FirstOrDefault(m => m.Id == id);
            return Task.FromResult(model);
        }

        public Task UpdateModelAsync(VehicleModelVM model)
        {
            var existing = _models.FirstOrDefault(m => m.Id == model.Id);
            if (existing != null)
            {
                existing.Name = model.Name;
                existing.Abrv = model.Abrv;
                existing.VehicleMakeId = model.VehicleMakeId;
            }
            return Task.CompletedTask;
        }

        public Task DeleteModelAsync(int id)
        {
            var model = _models.FirstOrDefault(m => m.Id == id);
            if (model != null) _models.Remove(model);
            return Task.CompletedTask;
        }

        public Task<IEnumerable<VehicleMakeVM>> GetAllMakesAsync()
        {
            return Task.FromResult(_makes.AsEnumerable());
        }

        public Task CreateMakeAsync(VehicleMakeVM make)
        {
            make.Id = _makes.Count + 1;
            _makes.Add(make);
            return Task.CompletedTask;
        }

        public Task<VehicleMakeVM> GetMakeByIdAsync(int id)
        {
            var make = _makes.FirstOrDefault(m => m.Id == id);
            return Task.FromResult(make);
        }

        public Task UpdateMakeAsync(VehicleMakeVM make)
        {
            var existing = _makes.FirstOrDefault(m => m.Id == make.Id);
            if (existing != null)
            {
                existing.Name = make.Name;
                existing.Abrv = make.Abrv;
            }
            return Task.CompletedTask;
        }

        public Task DeleteMakeAsync(int id)
        {
            var make = _makes.FirstOrDefault(m => m.Id == id);
            if (make != null) _makes.Remove(make);
            return Task.CompletedTask;
        }
    }
}
