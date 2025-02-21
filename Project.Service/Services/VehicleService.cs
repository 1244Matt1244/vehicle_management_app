using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Project.Service.Data.Context;
using Project.Service.Data.DTOs;
using Project.Service.Data.Helpers;
using Project.Service.Interfaces;

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

    // Get all vehicle models with pagination
    public async Task<PaginatedList<VehicleModelDTO>> GetModelsAsync(int page, int pageSize, string sortOrder, int makeId = 0)
    {
        if (page < 1) throw new ArgumentOutOfRangeException(nameof(page), "Page must be greater than 0.");
        if (pageSize < 1) throw new ArgumentOutOfRangeException(nameof(pageSize), "Page size must be greater than 0.");
        if (sortOrder != "asc" && sortOrder != "desc") throw new ArgumentException("SortOrder must be 'asc' or 'desc'.");

        var query = _context.VehicleModels.AsNoTracking();

        if (makeId > 0)
            query = query.Where(m => m.MakeId == makeId);

        query = sortOrder == "desc"
            ? query.OrderByDescending(m => m.Name)
            : query.OrderBy(m => m.Name);

        var totalCount = await query.CountAsync();
        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ProjectTo<VehicleModelDTO>(_mapper.ConfigurationProvider)
            .ToListAsync();

        return new PaginatedList<VehicleModelDTO>(items, totalCount, page, pageSize);
    }

    // Get a vehicle model by ID
    public async Task<VehicleModelDTO> GetModelByIdAsync(int id)
    {
        var model = await _context.VehicleModels
            .AsNoTracking()
            .Where(m => m.Id == id)
            .ProjectTo<VehicleModelDTO>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();

        if (model == null) throw new KeyNotFoundException($"Vehicle model with ID {id} not found.");
        return model;
    }

    // Add a new vehicle model
    public async Task AddModelAsync(VehicleModelDTO model)
    {
        var entity = _mapper.Map<VehicleModel>(model);
        _context.VehicleModels.Add(entity);
        await _context.SaveChangesAsync();
    }

    // Update an existing vehicle model
    public async Task UpdateModelAsync(VehicleModelDTO model)
    {
        var entity = await _context.VehicleModels.FindAsync(model.Id);
        if (entity == null) throw new KeyNotFoundException($"Vehicle model with ID {model.Id} not found.");

        _mapper.Map(model, entity);
        await _context.SaveChangesAsync();
    }

    // Delete a vehicle model
    public async Task DeleteModelAsync(int id)
    {
        var entity = await _context.VehicleModels.FindAsync(id);
        if (entity == null) throw new KeyNotFoundException($"Vehicle model with ID {id} not found.");

        _context.VehicleModels.Remove(entity);
        await _context.SaveChangesAsync();
    }

    // Get all vehicle makes with pagination
    public async Task<PaginatedList<VehicleMakeDTO>> GetMakesAsync(int page, int pageSize, string sortOrder)
    {
        if (page < 1) throw new ArgumentOutOfRangeException(nameof(page), "Page must be greater than 0.");
        if (pageSize < 1) throw new ArgumentOutOfRangeException(nameof(pageSize), "Page size must be greater than 0.");
        if (sortOrder != "asc" && sortOrder != "desc") throw new ArgumentException("SortOrder must be 'asc' or 'desc'.");

        var query = _context.VehicleMakes.AsNoTracking();

        query = sortOrder == "desc"
            ? query.OrderByDescending(m => m.Name)
            : query.OrderBy(m => m.Name);

        var totalCount = await query.CountAsync();
        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ProjectTo<VehicleMakeDTO>(_mapper.ConfigurationProvider)
            .ToListAsync();

        return new PaginatedList<VehicleMakeDTO>(items, totalCount, page, pageSize);
    }

    // Get a vehicle make by ID
    public async Task<VehicleMakeDTO> GetMakeByIdAsync(int id)
    {
        var make = await _context.VehicleMakes
            .AsNoTracking()
            .Where(m => m.Id == id)
            .ProjectTo<VehicleMakeDTO>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();

        if (make == null) throw new KeyNotFoundException($"Vehicle make with ID {id} not found.");
        return make;
    }

    // Create a new vehicle make
    public async Task CreateMakeAsync(VehicleMakeDTO makeDto)
    {
        var entity = _mapper.Map<VehicleMake>(makeDto);
        _context.VehicleMakes.Add(entity);
        await _context.SaveChangesAsync();
    }

    // Update an existing vehicle make
    public async Task UpdateMakeAsync(VehicleMakeDTO makeDto)
    {
        var entity = await _context.VehicleMakes.FindAsync(makeDto.Id);
        if (entity == null) throw new KeyNotFoundException($"Vehicle make with ID {makeDto.Id} not found.");

        _mapper.Map(makeDto, entity);
        await _context.SaveChangesAsync();
    }

    // Delete a vehicle make
    public async Task DeleteMakeAsync(int id)
    {
        var entity = await _context.VehicleMakes.FindAsync(id);
        if (entity == null) throw new KeyNotFoundException($"Vehicle make with ID {id} not found.");

        _context.VehicleMakes.Remove(entity);
        await _context.SaveChangesAsync();
    }
}
}