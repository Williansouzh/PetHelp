using PetHelp.Domain.Entities;
using static PetHelp.Domain.Enum.AnimalEnums;

namespace PetHelp.Domain.Interfaces.Repositories;

public interface IAnimalRepository : IRepository<Animal>
{
    Task<int> CountByUser(string userId);
    Task<int> CountByUserAndStatus(string userId, AnimalStatus status);
    Task<IEnumerable<Animal>> GetRecentByUser(string userId, int limit);
    Task<(IEnumerable<Animal> data, int totalCount)> GetAllByUser(string userId, int page, int pageSize);
    Task<IEnumerable<Animal>> GetByStatus(AnimalStatus status);
    Task<IEnumerable<Animal>> GetBySpecies(string species);
    Task<IEnumerable<Animal>> GetBySize(Size size);


}
