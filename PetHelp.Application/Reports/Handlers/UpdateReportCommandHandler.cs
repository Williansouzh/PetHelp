using AutoMapper;
using MediatR;
using PetHelp.Application.DTOs.Report;
using PetHelp.Application.Reports.Commands;
using PetHelp.Domain.Interfaces.Repositories;
using PetHelp.Domain.Interfaces.Services;

namespace PetHelp.Application.Reports.Handlers;

public class UpdateReportCommandHandler : IRequestHandler<UpdateReportCommand, ReportDTO>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IReportRepository _reportRepository;
    private readonly IMapper _mapper;
    public UpdateReportCommandHandler(IUnitOfWork unitOfWork, IReportRepository reportRepository, IMapper mapper)
    {
        _reportRepository = reportRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ReportDTO> Handle(UpdateReportCommand request, CancellationToken cancellationToken)
    {
        var report = await _reportRepository.GetByIdAsync(request.Id);
        if (report == null)
            throw new KeyNotFoundException($"Report with ID {request.Id} not found.");

        report.Update(
            request.Description ?? report.Description,
            request.Latitude != default ? request.Latitude : report.Latitude,
            request.Longitude != default ? request.Longitude : report.Longitude,
            request.Address ?? report.Address,
            request.AnimalType != default ? request.AnimalType : report.AnimalType,
            request.UrgencyLevel != default ? request.UrgencyLevel : report.UrgencyLevel,
            request.Name ?? report.Name,
            request.Phone ?? report.Phone,
            request.Email ?? report.Email,
            request.ImageUrl ?? report.ImageUrl
        );

        await _reportRepository.UpdateAsync(report);
        await _unitOfWork.CommitAsync();
        return _mapper.Map<ReportDTO>(report);
    }

}
