using PetHelp.Domain.Validation;
using static PetHelp.Domain.Enum.AdoptionEnums;

namespace PetHelp.Domain.Entities;


public class Adoption : Entity
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

    public AdoptionStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public Adoption() { }

    public Adoption(
        Guid animalId,
        string userId,
        string fullName,
        string email,
        string phone,
        string address,
        bool hasOtherPets,
        string housingType,
        int numberOfResidents,
        string workSchedule,
        string reasonForAdoption,
        bool agreedToTerms)
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;

        ValidateDomain(
            animalId,
            userId,
            fullName,
            email,
            phone,
            address,
            hasOtherPets,
            housingType,
            numberOfResidents,
            workSchedule,
            reasonForAdoption,
            agreedToTerms
        );

        Status = AdoptionStatus.Pending;
    }

    private void ValidateDomain(
        Guid animalId,
        string userId,
        string fullName,
        string email,
        string phone,
        string address,
        bool hasOtherPets,
        string housingType,
        int numberOfResidents,
        string workSchedule,
        string reasonForAdoption,
        bool agreedToTerms)
    {
        DomainExceptValidation.When(animalId == Guid.Empty, "AnimalId is required");
        DomainExceptValidation.When(string.IsNullOrWhiteSpace(userId), "UserId is required");
        DomainExceptValidation.When(string.IsNullOrWhiteSpace(fullName), "Full name is required");
        DomainExceptValidation.When(string.IsNullOrWhiteSpace(email), "Email is required");
        DomainExceptValidation.When(string.IsNullOrWhiteSpace(phone), "Phone is required");
        DomainExceptValidation.When(string.IsNullOrWhiteSpace(address), "Address is required");
        DomainExceptValidation.When(string.IsNullOrWhiteSpace(housingType), "Housing type is required");
        DomainExceptValidation.When(numberOfResidents <= 0, "Number of residents must be greater than 0");
        DomainExceptValidation.When(string.IsNullOrWhiteSpace(workSchedule), "Work schedule is required");
        DomainExceptValidation.When(string.IsNullOrWhiteSpace(reasonForAdoption), "Reason for adoption is required");
        DomainExceptValidation.When(!agreedToTerms, "You must agree to the adoption terms");

        AnimalId = animalId;
        UserId = userId;
        FullName = fullName;
        Email = email;
        Phone = phone;
        Address = address;
        HasOtherPets = hasOtherPets;
        HousingType = housingType;
        NumberOfResidents = numberOfResidents;
        WorkSchedule = workSchedule;
        ReasonForAdoption = reasonForAdoption;
        AgreedToTerms = agreedToTerms;
    }

    public void Approve() => Status = AdoptionStatus.Approved;

    public void Reject() => Status = AdoptionStatus.Rejected;
}
