using Microsoft.EntityFrameworkCore;
using Project.Service.Data.Context;
using Project.Service.Data.DTOs;
using Project.Service.Data.Helpers;
using Project.Service.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Service.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly ApplicationDbContext _context;

        public VehicleService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PaginatedList<VehicleMakeDTO>> GetMakesAsync(
            int pageIndex, 
            int pageSize, 
            string sortBy, 
            string sortOrder, 
            string searchString)
        {
            var query = _context.VehicleMakes.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                query = query.Where(m => 
                    m.Name.Contains(searchString) || 
                    m.Abbreviation.Contains(searchString));
            }

            query = query.OrderByProperty(sortBy, sortOrder);

            return await PaginatedList<VehicleMakeDTO>.CreateAsync(
                query.Select(m => new VehicleMakeDTO 
                { 
                    Id = m.Id,
                    Name = m.Name,
                    Abbreviation = m.Abbreviation
                }), 
                pageIndex, 
                pageSize
            );
        }
    }
}