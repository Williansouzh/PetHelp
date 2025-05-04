using PetHelp.Domain.Validation;

namespace PetHelp.Domain.Entities;

public class Animal : Entity
{
    public string Name { get; set; }
    public string Species { get; set; } 
    public string Breed { get; set; }
    public int Age { get; set; }
    public string Description { get; set; }
    public string? ImageUrl { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string CreatedByUserId { get; set; }
    public Animal() { }
    public Animal(string name, string species, string breed, int age, string description, string imageUrl, string city, string state, string createdByUserId)
    {
        Id = Guid.NewGuid();
        ValidateDomain(name, species, breed, age, description, imageUrl, city, state, createdByUserId);
        CreatedByUserId = createdByUserId;
    }
    public Animal(Guid id, string name, string species, string breed, int age, string description, string imageUrl, string city, string state, string createdByUserId)
    {
        DomainExceptValidation.When(id == Guid.Empty, "Id is required");
        Id = id;
        ValidateDomain(name, species, breed, age, description, imageUrl, city, state, createdByUserId);   
    }
    public void ValidateDomain(string name, string species, string breed, int age, string description, string imageUrl, string city, string state, string createdByUserId)
    {
        DomainExceptValidation.When(string.IsNullOrEmpty(name), "Name is required");
        DomainExceptValidation.When(string.IsNullOrEmpty(species), "Species is required");
        DomainExceptValidation.When(string.IsNullOrEmpty(breed), "Breed is required");
        DomainExceptValidation.When(age < 0, "Age must be greater than or equal to 0");
        DomainExceptValidation.When(string.IsNullOrEmpty(description), "Description is required");
        //DomainExceptValidation.When(string.IsNullOrEmpty(imageUrl), "ImageUrl is required");
        DomainExceptValidation.When(string.IsNullOrEmpty(city), "City is required");
        DomainExceptValidation.When(string.IsNullOrEmpty(state), "State is required");
        DomainExceptValidation.When(string.IsNullOrEmpty(createdByUserId), "CreatedByUserId Required");
        Name = name;
        Species = species;
        Breed = breed;
        Age = age;
        Description = description;
        ImageUrl = imageUrl;
        City = city;
        State = state;
    }
    public void Update(string name, string species, string breed, int age, string description, string imageUrl, string city, string state)
    {
        ValidateDomain(name, species, breed, age, description, imageUrl, city, state, CreatedByUserId);
    }
}   
