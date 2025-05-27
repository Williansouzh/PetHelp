
namespace PetHelp.API.Controllers;

public class ReportDashboardDto
{
    public Guid Id { get; set; }
    public string UrgencyLevel { get; set; }
    public string AnimalType { get; set; }
    public DateTime CreatedAt { get; set; }
}