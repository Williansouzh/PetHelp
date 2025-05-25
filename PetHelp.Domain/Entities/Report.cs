using PetHelp.Domain.Validation;
using static PetHelp.Domain.Enum.ReportEnums;

namespace PetHelp.Domain.Entities;

public class Report
{
    public Guid Id { get; private set; }

    public string Description { get; private set; }
    public string? ImageUrl { get; private set; }

    public float Latitude { get; private set; }
    public float Longitude { get; private set; }

    public string Address { get; private set; }
    public AnimalType AnimalType { get; private set; }
    public UrgencyLevel UrgencyLevel { get; private set; }

    public Guid? UserId { get; private set; } // Pode ser nulo para denúncia anônima

    public string? Name { get; private set; }
    public string? Phone { get; private set; }
    public string? Email { get; private set; }

    protected Report() { }

    public Report(
        string description,
        string? imageUrl,
        float latitude,
        float longitude,
        string address,
        AnimalType animalType,
        UrgencyLevel urgencyLevel,
        Guid? userId,
        string name,
        string phone,
        string email)
    {
        Id = Guid.NewGuid();
        ValidateDomain(description, latitude, longitude, address, animalType, urgencyLevel, name, phone, email);

        ImageUrl = imageUrl;
        UserId = userId;
    }

    public void Update(
        string description,
        float latitude,
        float longitude,
        string address,
        AnimalType animalType,
        UrgencyLevel urgencyLevel,
        string name,
        string phone,
        string email,
        string? imageUrl = null)
    {
        ValidateDomain(description, latitude, longitude, address, animalType, urgencyLevel, name, phone, email);

        ImageUrl = imageUrl;
    }

    private void ValidateDomain(
        string description,
        float latitude,
        float longitude,
        string address,
        AnimalType animalType,
        UrgencyLevel urgencyLevel,
        string name,
        string phone,
        string email)
    {
        DomainExceptValidation.When(string.IsNullOrWhiteSpace(description), "Description is required");
        DomainExceptValidation.When(string.IsNullOrWhiteSpace(address), "Address is required");
        DomainExceptValidation.When(latitude == 0, "Latitude is required");
        DomainExceptValidation.When(longitude == 0, "Longitude is required");
        DomainExceptValidation.When(string.IsNullOrWhiteSpace(name), "Name is required");
        DomainExceptValidation.When(string.IsNullOrWhiteSpace(phone), "Phone is required");
        DomainExceptValidation.When(string.IsNullOrWhiteSpace(email), "Email is required");

        Description = description.Trim();
        Latitude = latitude;
        Longitude = longitude;
        Address = address.Trim();
        AnimalType = animalType;
        UrgencyLevel = urgencyLevel;
        Name = name.Trim();
        Phone = phone.Trim();
        Email = email.Trim();
    }
}
