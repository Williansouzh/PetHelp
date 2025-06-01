using static PetHelp.Domain.Enum.ReportEnums;

namespace PetHelp.Application.DTOs.Dashboard;

public class PetDashboardDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Breed { get; set; }
    public int Age { get; set; }
    public string Status { get; set; }
    public string ImageUrl { get; set; }
    public DateTime CreatedAt { get; set; }
    public AnimalType AnimalType { get; set; }
}
