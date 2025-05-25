using MediatR;
using PetHelp.Application.DTOs.Report;
using static PetHelp.Domain.Enum.ReportEnums;

namespace PetHelp.Application.Reports.Commands;

public abstract class ReportCommand : IRequest<ReportDTO>
{
    public string Description { get; private set; }
    public string? ImageUrl { get; private set; }
    public float Latitude { get; private set; }
    public float Longitude { get; private set; }
    public string Address { get; private set; }
    public AnimalType AnimalType { get; private set; }
    public UrgencyLevel UrgencyLevel { get; private set; }

    public Guid? UserId { get; private set; }
    public string Name { get; private set; }
    public string Phone { get; private set; }
    public string Email { get; private set; }
}
