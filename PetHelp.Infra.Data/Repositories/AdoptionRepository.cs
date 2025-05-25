using PetHelp.Domain.Entities;
using PetHelp.Domain.Interfaces.Repositories;
using PetHelp.Infra.Data.Context;

namespace PetHelp.Infra.Data.Repositories;

public class AdoptionRepository : Repository<Adoption>, IAdoptionRepository
{
    private readonly ApplicationDbContext _context;
    public AdoptionRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }
}
