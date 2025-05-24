namespace PetHelp.Domain.Interfaces.Repositories;

public  interface IRepository<T> where T : class
{
    Task<IReadOnlyCollection<T>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<T> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);
    Task<T> UpdateAsync(T entity, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> DeleteAllAsync(CancellationToken cancellationToken = default);
}
