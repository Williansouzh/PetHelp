using MediatR;
using PetHelp.Domain.Entities;
namespace PetHelp.Application.Queries.Animals;

public class GetAnimalsQuery : IRequest<IEnumerable<Animal>>
{

}
