using System.Linq.Expressions;
using MediatR;
using PetHelp.Application.DTOs.Report;
using PetHelp.Application.Pagination;
using PetHelp.Domain.Entities;

namespace PetHelp.Application.Reports.Queries;

public class GetReportsQuery : IRequest<PaginationResponse<ReportDTO>>
{
    public PaginationRequest Pagination { get; }
    public Expression<Func<Report, bool>> Filter { get; }
    public Func<IQueryable<Report>, IOrderedQueryable<Report>> OrderBy { get; }
    public GetReportsQuery(
        PaginationRequest pagination,
        Expression<Func<Report, bool>> filter = null,
        Func<IQueryable<Report>, IOrderedQueryable<Report>> orderBy = null)
    {
        Pagination = pagination;
        Filter = filter;
        OrderBy = orderBy;
    }
}
