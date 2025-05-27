using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using PetHelp.Domain.Entities;
using PetHelp.Domain.Interfaces.Repositories;
using PetHelp.Infra.Data.Context;
using static PetHelp.Domain.Enum.AnimalEnums;

namespace PetHelp.Infra.Data.Repositories;

public class AnimalRepository : Repository<Animal>, IAnimalRepository
{
    private readonly ApplicationDbContext _context;
    public AnimalRepository(ApplicationDbContext context ) : base(context)
    {
        _context = context; 
    }
    public async Task<int> CountByUser(string userId)
    {
        return await _context.Pets
            .Where(a => a.CreatedByUserId == userId)
            .CountAsync();
    }

    public async Task<int> CountByUserAndStatus(string userId, AnimalStatus status)
    {
        return await _context.Pets
            .Where(a => a.CreatedByUserId == userId && a.Status == status)
            .CountAsync();
    }

    public async Task<IEnumerable<Animal>> GetRecentByUser(string userId, int limit)
    {
        return await _context.Pets
            .Where(a => a.CreatedByUserId == userId)
            .OrderByDescending(a => a.CreatedAt)
            .Take(limit)
            .ToListAsync();
    }

    public async Task<(IEnumerable<Animal> data, int totalCount)> GetAllByUser(string userId, int page, int pageSize)
    {
        var query = _context.Pets
            .Where(a => a.CreatedByUserId == userId);

        var totalCount = await query.CountAsync();

        var data = await query
            .OrderByDescending(a => a.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (data, totalCount);
    }

    public async Task<IEnumerable<Animal>> GetByStatus(AnimalStatus status)
    {
        return await _context.Pets
            .Where(a => a.Status == status)
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Animal>> GetBySpecies(string species)
    {
        return await _context.Pets
            .Where(a => a.Species.ToLower() == species.ToLower())
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Animal>> GetBySize(Size size)
    {
        return await _context.Pets
            .Where(a => a.Size == size)
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync();
    }
}
