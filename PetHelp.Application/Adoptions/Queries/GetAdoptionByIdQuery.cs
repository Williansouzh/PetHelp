using MediatR;
using PetHelp.Domain.Entities;

namespace PetHelp.Application.Adoptions.Queries;

public class GetAdoptionByIdQuery : IRequest<Adoption>
{
    public Guid Id { get; set; }
    public GetAdoptionByIdQuery(Guid id)
    {
        Id = id;
    }
}
