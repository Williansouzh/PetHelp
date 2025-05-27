using System.Linq.Expressions;
using MediatR;
using PetHelp.Application.DTOs.Adoption;
using PetHelp.Application.Pagination;
using PetHelp.Domain.Entities;

namespace PetHelp.Application.Adoptions.Queries;

public class GetAdoptionsQuery : IRequest<PaginationResponse<AdoptionDTO>>
{
    public PaginationRequest Pagination { get; }
    public Expression<Func<Adoption, bool>> Filter { get; }
    public Func<IQueryable<Adoption>, IOrderedQueryable<Adoption>> OrderBy { get; }
    public GetAdoptionsQuery(
        PaginationRequest pagination,
        Expression<Func<Adoption, bool>> filter = null,
        Func<IQueryable<Adoption>, IOrderedQueryable<Adoption>> orderBy = null)
    {
        Pagination = pagination;
        Filter = filter;
        OrderBy = orderBy;
    }
}
