using System.Linq.Expressions;
using MediatR;
using PetHelp.Application.DTOs.Animal;
using PetHelp.Application.Pagination;
using PetHelp.Domain.Entities;
namespace PetHelp.Application.Animals.Queries;

public class GetAnimalsQuery : IRequest<PaginationResponse<AnimalDTO>>
{
    public PaginationRequest Pagination { get; }
    public Expression<Func<Animal, bool>> Filter { get; }
    public Func<IQueryable<Animal>, IOrderedQueryable<Animal>> OrderBy { get; }

    public GetAnimalsQuery(
        PaginationRequest pagination,
        Expression<Func<Animal, bool>> filter = null,
        Func<IQueryable<Animal>, IOrderedQueryable<Animal>> orderBy = null)
    {
        Pagination = pagination;
        Filter = filter;
        OrderBy = orderBy;
    }
}
