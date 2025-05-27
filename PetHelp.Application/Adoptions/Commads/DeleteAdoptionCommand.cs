using MediatR;
using PetHelp.Application.DTOs.Adoption;

namespace PetHelp.Application.Adoptions.Commads;

public class DeleteAdoptionCommand : IRequest<AdoptionDTO>
{
    public Guid Id { get; private set; }
    public DeleteAdoptionCommand(Guid id)
    {
        Id = id;
    }
}
