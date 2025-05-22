using PetHelp.Domain.Interfaces.Repositories;
using PetHelp.Domain.Interfaces.Services;
using PetHelp.Infra.Data.Context;

namespace PetHelp.Infra.Data.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    public IAnimalRepository AnimalRepository { get; }

    public UnitOfWork(ApplicationDbContext context, IAnimalRepository animals)
    {
        _context = context;
        AnimalRepository = animals;

    }
    public async Task<int> CommitAsync()
    {
        return await _context.SaveChangesAsync();
    }
    public void Dispose()
    {
        _context.Dispose();
    }
}
