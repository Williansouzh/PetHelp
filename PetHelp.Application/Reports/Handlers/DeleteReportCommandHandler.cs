using AutoMapper;
using MediatR;
using PetHelp.Application.DTOs.Report;
using PetHelp.Application.Reports.Commands;
using PetHelp.Domain.Interfaces.Repositories;
using PetHelp.Domain.Interfaces.Services;

namespace PetHelp.Application.Reports.Handlers;

public class DeleteReportCommandHandler : IRequestHandler<DeleteReportCommand, ReportDTO>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IReportRepository _reportRepository;
    private readonly IMapper _mapper;
    public DeleteReportCommandHandler(IUnitOfWork unitOfWork, IReportRepository reportRepository, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _reportRepository = reportRepository;
        _mapper = mapper;
    }

    public async Task<ReportDTO> Handle(DeleteReportCommand request, CancellationToken cancellationToken)
    {
        var report = await _reportRepository.GetByIdAsync(request.Id);
        if (report == null)
        {
            throw new KeyNotFoundException($"Report with ID {request.Id} not found.");
        }
        await _reportRepository.DeleteAsync(report.Id);
        await _unitOfWork.CommitAsync();
        return _mapper.Map<ReportDTO>(report);
    }
}
