using PetHelp.Domain.Entities;
using PetHelp.Domain.Interfaces.Repositories;
using PetHelp.Infra.Data.Context;

namespace PetHelp.Infra.Data.Repositories;

public class ReportRepository : Repository<Report>, IReportRepository
{
    private readonly ApplicationDbContext _context;
    public ReportRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }
}
