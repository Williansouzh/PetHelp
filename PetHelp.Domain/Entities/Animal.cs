using PetHelp.Domain.Validation;
using static PetHelp.Domain.Enum.AnimalEnums;

namespace PetHelp.Domain.Entities;

public class Animal : Entity
{
    public string Name { get; set; }
    public string Species { get; set; }
    public string Breed { get; set; }
    public DateTime BirthDate { get; set; }
    public Gender Gender { get; set; }
    public Size Size { get; set; }
    public string Description { get; set; }
    public bool IsVaccinated { get; set; }
    public bool IsNeutered { get; set; }
    public string AdoptionRequirements { get; set; }
    public AnimalStatus Status { get; set; }
    public List<string> PhotoUrls { get; set; } = new();
    public string? ImageUrl { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string CreatedByUserId { get; set; }

    public Animal() { }

    public Animal(string name, string species, string breed, DateTime birthDate, Gender gender, Size size,
        string description, bool isVaccinated, bool isNeutered, string adoptionRequirements, AnimalStatus status,
        List<string> photoUrls, string imageUrl, string city, string state, string createdByUserId)
    {
        Id = Guid.NewGuid();
        ValidateDomain(name, species, breed, birthDate, gender, size, description,
            isVaccinated, isNeutered, adoptionRequirements, status,
            photoUrls, imageUrl, city, state, createdByUserId);

        CreatedByUserId = createdByUserId;
    }

    public void ValidateDomain(string name, string species, string breed, DateTime birthDate, Gender gender, Size size,
        string description, bool isVaccinated, bool isNeutered, string adoptionRequirements, AnimalStatus status,
        List<string> photoUrls, string imageUrl, string city, string state, string createdByUserId)
    {
        DomainExceptValidation.When(string.IsNullOrWhiteSpace(name), "Name is required");
        DomainExceptValidation.When(string.IsNullOrWhiteSpace(species), "Species is required");
        DomainExceptValidation.When(string.IsNullOrWhiteSpace(breed), "Breed is required");
        DomainExceptValidation.When(birthDate > DateTime.UtcNow, "BirthDate cannot be in the future");
        DomainExceptValidation.When(string.IsNullOrWhiteSpace(description), "Description is required");
        DomainExceptValidation.When(string.IsNullOrWhiteSpace(adoptionRequirements), "Adoption requirements are required");
        DomainExceptValidation.When(photoUrls.Count > 5, "You can upload up to 5 photos");
        DomainExceptValidation.When(string.IsNullOrWhiteSpace(city), "City is required");
        DomainExceptValidation.When(string.IsNullOrWhiteSpace(state), "State is required");
        DomainExceptValidation.When(string.IsNullOrWhiteSpace(createdByUserId), "CreatedByUserId is required");

        Name = name;
        Species = species;
        Breed = breed;
        BirthDate = birthDate;
        Gender = gender;
        Size = size;
        Description = description;
        IsVaccinated = isVaccinated;
        IsNeutered = isNeutered;
        AdoptionRequirements = adoptionRequirements;
        Status = status;
        PhotoUrls = photoUrls;
        ImageUrl = imageUrl;
        City = city;
        State = state;
    }

    public void Update(string name, string species, string breed, DateTime birthDate, Gender gender, Size size,
        string description, bool isVaccinated, bool isNeutered, string adoptionRequirements, AnimalStatus status,
        List<string> photoUrls, string imageUrl, string city, string state)
    {
        ValidateDomain(name, species, breed, birthDate, gender, size, description,
            isVaccinated, isNeutered, adoptionRequirements, status,
            photoUrls, imageUrl, city, state, CreatedByUserId);
    }
}
