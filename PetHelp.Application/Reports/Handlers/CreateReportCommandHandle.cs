
using AutoMapper;
using MediatR;
using PetHelp.Application.DTOs.Report;
using PetHelp.Application.Reports.Commands;
using PetHelp.Domain.Entities;
using PetHelp.Domain.Interfaces.Repositories;
using PetHelp.Domain.Interfaces.Services;

namespace PetHelp.Application.Reports.Handlers;

public class CreateReportCommandHandle : IRequestHandler<CreateReportCommand, ReportDTO>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IReportRepository _reportRepository;
    private readonly IMapper _mapper;
    public CreateReportCommandHandle(IUnitOfWork unitOfWork, IReportRepository reportRepository, IMapper mapper)
    {
        _reportRepository = reportRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ReportDTO> Handle(CreateReportCommand request, CancellationToken cancellationToken)
    {
        var report = new Report(request.Description, request.ImageUrl, request.Latitude, request.Longitude, request.Address, request.AnimalType, request.UrgencyLevel, request.UserId, request.Name, request.Phone, request.Email);
        var result = await _reportRepository.AddAsync(report, cancellationToken);
        await _unitOfWork.CommitAsync();
        return _mapper.Map<ReportDTO>(report);
    }
}
