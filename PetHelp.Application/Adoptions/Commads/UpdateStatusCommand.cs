using MediatR;
using PetHelp.Application.DTOs.Adoption;
using static PetHelp.Domain.Enum.AdoptionEnums;

namespace PetHelp.Application.Adoptions.Commads;

public class UpdateStatusCommand : IRequest<AdoptionDTO>
{
    public Guid AdoptionId { get; set; }
    public string UserId { get; set; }
    public AdoptionStatus Status { get; set; } 
}
