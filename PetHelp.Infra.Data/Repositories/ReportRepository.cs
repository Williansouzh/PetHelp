using Microsoft.EntityFrameworkCore;
using PetHelp.Domain.Entities;
using PetHelp.Domain.Interfaces.Repositories;
using PetHelp.Infra.Data.Context;
using static PetHelp.Domain.Enum.ReportEnums;

namespace PetHelp.Infra.Data.Repositories;

public class ReportRepository : Repository<Report>, IReportRepository
{
    private readonly ApplicationDbContext _context;
    public ReportRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }
    public async Task<IEnumerable<Report>> GetByUrgencyLevel(UrgencyLevel urgencyLevel)
    {
        return await _context.Reports
            .Where(r => r.UrgencyLevel == urgencyLevel)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Report>> GetByAnimalType(AnimalType animalType)
    {
        return await _context.Reports
            .Where(r => r.AnimalType == animalType)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Report>> GetRecentReports(int limit)
    {
        return await _context.Reports
            .OrderByDescending(r => r.CreatedAt)
            .Take(limit)
            .ToListAsync();
    }
}
