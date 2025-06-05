using Microsoft.EntityFrameworkCore;
using PetHelp.Infra.Data.Context;

namespace PetHelp.Infra.Data.Migrations;

public class MigrationService : IMigrationService
{
    private readonly ApplicationDbContext _context;

    public MigrationService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task MigrateAsync()
    {
        await _context.Database.MigrateAsync();
    }
}
