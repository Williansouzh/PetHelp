using PetHelp.Domain.Entities;
using static PetHelp.Domain.Enum.ReportEnums;

namespace PetHelp.Domain.Interfaces.Repositories;

public interface IReportRepository : IRepository<Report>
{
    Task<IEnumerable<Report>> GetByUrgencyLevel(UrgencyLevel urgencyLevel);
    Task<IEnumerable<Report>> GetByAnimalType(AnimalType animalType);
    Task<IEnumerable<Report>> GetRecentReports(int limit);
}
