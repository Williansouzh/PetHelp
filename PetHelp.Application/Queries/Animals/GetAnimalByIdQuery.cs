using MediatR;
using PetHelp.Domain.Entities;
namespace PetHelp.Application.Queries.Animals;

public class GetAnimalByIdQuery : IRequest<Animal>
{
    public Guid Id { get; set; }
    public GetAnimalByIdQuery(Guid id)
    {
        Id = id;
    }
}
