using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using PetHelp.Domain.Entities;
using PetHelp.Domain.Interfaces.Repositories;
using PetHelp.Infra.Data.Context;

namespace PetHelp.Infra.Data.Repositories;

public class AnimalRepository : Repository<Animal>, IAnimalRepository
{
    private readonly ApplicationDbContext _context;
    public AnimalRepository(ApplicationDbContext context ) : base(context)
    {
        _context = context; 
    }

    public async Task<(IReadOnlyList<Animal> data, int totalCount)> GetPaginatedAsync(
     int pageNumber,
     int pageSize,
     Expression<Func<Animal, bool>> filter = null,
     Func<IQueryable<Animal>, IOrderedQueryable<Animal>> orderBy = null,
     CancellationToken cancellationToken = default)
    {
        IQueryable<Animal> query = _context.Pets;

        if (filter != null)
        {
            query = query.Where(filter);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        if (orderBy != null)
        {
            query = orderBy(query);
        }

        var data = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (data, totalCount);
    }
}
