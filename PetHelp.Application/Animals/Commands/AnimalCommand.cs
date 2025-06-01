using MediatR;
using PetHelp.Domain.Entities;
using static PetHelp.Domain.Enum.AnimalEnums;
using static PetHelp.Domain.Enum.ReportEnums;
namespace PetHelp.Application.Animals.Commands;
public abstract class AnimalCommand : IRequest<Animal>
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
    public AnimalType AnimalType { get; set; }
}
