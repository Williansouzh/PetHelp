using MediatR;
using PetHelp.Application.DTOs.Report;

namespace PetHelp.Application.Reports.Commands;

public class DeleteReportCommand : IRequest<ReportDTO>
{
    public Guid Id { get; set; }
}
