namespace PetHelp.Application.DTOs.Adoption;

public class AdoptionDTO
{
    public Guid AnimalId { get; private set; }
    public string UserId { get; private set; }

    public string FullName { get; private set; }
    public string Email { get; private set; }
    public string Phone { get; private set; }
    public string Address { get; private set; }
    public bool HasOtherPets { get; private set; }
    public string HousingType { get; private set; }
    public int NumberOfResidents { get; private set; }
    public string WorkSchedule { get; private set; }
    public string ReasonForAdoption { get; private set; }
    public bool AgreedToTerms { get; private set; }
}
