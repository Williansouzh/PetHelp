namespace PetHelp.Application.DTOs.Dashboard;

public class AdoptionRequestDashboardDto
{
    public int Id { get; set; }
    public string PetName { get; set; }
    public int PetId { get; set; }
    public string RequesterName { get; set; }
    public string RequesterEmail { get; set; }
    public string Status { get; set; }
    public DateTime CreatedAt { get; set; }
}
