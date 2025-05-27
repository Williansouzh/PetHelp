namespace PetHelp.Application.DTOs.Dashboard;

public class DashboardStatsDto
{
    public int TotalPets { get; set; }
    public int AdoptedPets { get; set; }
    public int AvailablePets { get; set; }
    public int PendingRequests { get; set; }
}
