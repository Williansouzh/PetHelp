
using static PetHelp.Domain.Enum.ReportEnums;

namespace PetHelp.API.Controllers;

public class ReportDashboardDto
{
    public Guid Id { get; set; }
    public string UrgencyLevel { get; set; }
    public AnimalType AnimalType { get; set; }
    public DateTime CreatedAt { get; set; }
}