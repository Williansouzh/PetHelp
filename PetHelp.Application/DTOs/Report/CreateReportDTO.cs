using static PetHelp.Domain.Enum.ReportEnums;

namespace PetHelp.Application.DTOs.Report;

public class CreateReportDTO
{
    public string Description { get;  set; }
    public string? ImageUrl { get;  set; }

    public float Latitude { get;  set; }
    public float Longitude { get;  set; }

    public string Address { get;  set; }
    public AnimalType AnimalType { get;  set; }
    public UrgencyLevel UrgencyLevel { get;  set; }

    public Guid? UserId { get;  set; }

    public string? Name { get;  set; }
    public string? Phone { get;  set; }
    public string? Email { get;  set; }
    public object Id { get; set; }
}
