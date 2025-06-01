using MediatR;
using PetHelp.Application.DTOs.Adoption;
using static PetHelp.Domain.Enum.AdoptionEnums;

namespace PetHelp.Application.Adoptions.Commads;

public class AdoptionCommand : IRequest<AdoptionDTO>
{
    public Guid AnimalId { get;  set; }
    public string UserId { get;  set; }

    public string FullName { get;  set; }
    public string AnimalName { get; set; }
    public string Email { get;  set; }
    public string Phone { get;  set; }
    public string Address { get;  set; }
    public bool HasOtherPets { get;  set; }
    public string HousingType { get;  set; }
    public int NumberOfResidents { get;  set; }
    public string WorkSchedule { get;  set; }
    public string ReasonForAdoption { get;  set; }
    public bool AgreedToTerms { get;  set; }
    public AdoptionStatus? Status { get; set; }
}
