using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Project.Service.Data;
using Project.Service.DTOs;
using Project.Service.Interfaces;
using Project.Service.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Service.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public VehicleService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<VehicleMakeDTO>> GetAllMakesAsync(string search, string sortOrder, int page, int pageSize)
        {
            var query = _context.VehicleMakes.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(m => m.Name.Contains(search) || m.Abrv.Contains(search));
            }

            query = sortOrder switch
            {
                "name_desc" => query.OrderByDescending(m => m.Name),
                _ => query.OrderBy(m => m.Name),
            };

            return _mapper.Map<IEnumerable<VehicleMakeDTO>>(await PaginatedList<VehicleMake>.CreateAsync(query, page, pageSize));
        }

        public async Task<IEnumerable<VehicleModelDTO>> GetAllModelsAsync(string search, string sortOrder, int page, int pageSize)
        {
            var query = _context.VehicleModels.Include(m => m.VehicleMake).AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(m => m.Name.Contains(search) || m.Abrv.Contains(search));
            }

            query = sortOrder switch
            {
                "name_desc" => query.OrderByDescending(m => m.Name),
                _ => query.OrderBy(m => m.Name),
            };

            return _mapper.Map<IEnumerable<VehicleModelDTO>>(await PaginatedList<VehicleModel>.CreateAsync(query, page, pageSize));
        }

        public async Task<VehicleMakeDTO> GetMakeByIdAsync(int id)
        {
            var make = await _context.VehicleMakes.FindAsync(id);
            return _mapper.Map<VehicleMakeDTO>(make);
        }

        public async Task<VehicleModelDTO> GetModelByIdAsync(int id)
        {
            var model = await _context.VehicleModels.Include(m => m.VehicleMake).FirstOrDefaultAsync(m => m.Id == id);
            return _mapper.Map<VehicleModelDTO>(model);
        }

        public async Task AddMakeAsync(VehicleMakeDTO makeDto)
        {
            var make = _mapper.Map<VehicleMake>(makeDto);
            _context.VehicleMakes.Add(make);
            await _context.SaveChangesAsync();
        }

        public async Task AddModelAsync(VehicleModelDTO modelDto)
        {
            var model = _mapper.Map<VehicleModel>(modelDto);
            _context.VehicleModels.Add(model);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateMakeAsync(VehicleMakeDTO makeDto)
        {
            var make = _mapper.Map<VehicleMake>(makeDto);
            _context.VehicleMakes.Update(make);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateModelAsync(VehicleModelDTO modelDto)
        {
            var model = _mapper.Map<VehicleModel>(modelDto);
            _context.VehicleModels.Update(model);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteMakeAsync(int id)
        {
            var make = await _context.VehicleMakes.FindAsync(id);
            if (make != null)
            {
                _context.VehicleMakes.Remove(make);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteModelAsync(int id)
        {
            var model = await _context.VehicleModels.FindAsync(id);
            if (model != null)
            {
                _context.VehicleModels.Remove(model);
                await _context.SaveChangesAsync();
            }
        }
    }
}
