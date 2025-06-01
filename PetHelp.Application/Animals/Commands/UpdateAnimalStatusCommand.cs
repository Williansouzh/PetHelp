using MediatR;
using PetHelp.Application.DTOs.Animal;
using static PetHelp.Domain.Enum.AnimalEnums;

namespace PetHelp.Application.Animals.Commands;

public class UpdateAnimalStatusCommand : IRequest<AnimalDTO>
{
    public Guid AnimalId { get; set; }
    public string UserId { get; set; }
    public AnimalStatus Status { get; set; } 
}
