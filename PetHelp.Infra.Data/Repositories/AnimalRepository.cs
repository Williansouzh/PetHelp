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
}
