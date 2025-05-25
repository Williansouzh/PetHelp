using static PetHelp.Domain.Enum.ReportEnums;

namespace PetHelp.API.DTOs.ReportDTOs;

public class ReportResponseDTO
{
    public Guid Id { get; set; }
    public string Description { get; set; }
    public string? ImageUrl { get; set; }
    public float Latitude { get; set; }
    public float Longitude { get; set; }
    public string Address { get; set; }
    public AnimalType AnimalType { get; set; }
    public UrgencyLevel UrgencyLevel { get; set; }
    public Guid? UserId { get; set; }
    public string Name { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public DateTime CreatedAt { get; set; }
}
