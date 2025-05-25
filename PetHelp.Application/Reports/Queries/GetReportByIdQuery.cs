using MediatR;
using PetHelp.Domain.Entities;

namespace PetHelp.Application.Reports.Queries;

public class GetReportByIdQuery : IRequest<Report>
{
    public Guid Id { get; set; }
    public GetReportByIdQuery(Guid id)
    {
        Id = id;
    }
}
