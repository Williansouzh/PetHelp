using MediatR;
using PetHelp.Domain.Entities;
namespace PetHelp.Application.Queries.Animal;

public class GetAnimalByIdQuery : IRequest<PetHelp.Domain.Entities.Animal>
{
    public Guid Id { get; set; }
    public GetAnimalByIdQuery(Guid id)
    {
        Id = id;
    }
}
