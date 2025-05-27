using PetHelp.Domain.Entities;

namespace PetHelp.Domain.Interfaces.Repositories;

public interface IAdoptionRepository : IRepository<Adoption>
{
    Task<int> CountPendingByOng(string ongId);
    Task<IEnumerable<Adoption>> GetRecentByOng(string ongId, int limit);
    Task<(IEnumerable<Adoption> data, int totalCount)> GetAllByOng(string ongId, int page, int pageSize);
}
