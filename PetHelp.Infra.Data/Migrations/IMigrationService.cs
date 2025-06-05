namespace PetHelp.Infra.Data.Migrations;

public interface IMigrationService
{
    Task MigrateAsync();
}
