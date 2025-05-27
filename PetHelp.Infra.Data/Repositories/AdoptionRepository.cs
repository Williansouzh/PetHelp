using Microsoft.EntityFrameworkCore;
using PetHelp.Domain.Entities;
using PetHelp.Domain.Interfaces.Repositories;
using PetHelp.Infra.Data.Context;
using static PetHelp.Domain.Enum.AdoptionEnums;

namespace PetHelp.Infra.Data.Repositories;

public class AdoptionRepository : Repository<Adoption>, IAdoptionRepository
{
    private readonly ApplicationDbContext _context;
    public AdoptionRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }
    public async Task<int> CountPendingByOng(string ongId)
    {
        return await _context.Adoption
            .Where(a => a.UserId == ongId && a.Status == AdoptionStatus.Pending)
            .CountAsync();
    }

    public async Task<IEnumerable<Adoption>> GetRecentByOng(string ongId, int limit)
    {
        return await _context.Adoption
            .Where(a => a.UserId == ongId)
            .Include(a => a.AnimalId)
            .Include(a => a)
            .OrderByDescending(a => a.CreatedAt)
            .Take(limit)
            .ToListAsync();
    }

    public async Task<(IEnumerable<Adoption> data, int totalCount)> GetAllByOng(string ongId, int page, int pageSize)
    {
        var query = _context.Adoption
            .Where(a => a.UserId == ongId)
            .Include(a => a.AnimalId);
            //.Include(a => a.Adopter);

        var totalCount = await query.CountAsync();

        var data = await query
            .OrderByDescending(a => a.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (data, totalCount);
    }
}
