using System.Linq.Expressions;
using PetHelp.Domain.Entities;

namespace PetHelp.Domain.Interfaces.Repositories
{
    public interface IAnimalRepository : IRepository<Animal>
    {
        Task<(IReadOnlyList<Animal> data, int totalCount)> GetPaginatedAsync(
        int pageNumber,
        int pageSize,
        Expression<Func<Animal, bool>> filter = null,
        Func<IQueryable<Animal>, IOrderedQueryable<Animal>> orderBy = null,
        CancellationToken cancellationToken = default);
    }
}
