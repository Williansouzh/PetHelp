using System.ComponentModel.DataAnnotations;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PetHelp.Application.DTOs.Report;
using PetHelp.Application.Interfaces;
using PetHelp.Application.Reports.Commands;
using PetHelp.Application.Reports.Queries;

namespace PetHelp.Application.Services;

public class ReportService : IReportService
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;
    private readonly ILogger<ReportService> _logger;
    public ReportService(IMapper mapper, IMediator mediator, ILogger<ReportService> logger)
    {
        _mapper = mapper;
        _mediator = mediator;
        _logger = logger;
    }

    public async Task<ReportDTO> CreateReportAsync(CreateReportDTO createReportDto)
    {
        _logger.LogInformation("Creating report with description: {Description}", createReportDto.Description);
        ValidateReportDTO(createReportDto);
        var command = _mapper.Map<CreateReportCommand>(createReportDto);
        var report = await _mediator.Send(command);
        _logger.LogInformation("Report created with description: {Description}", createReportDto.Description);
        return report;
    }

    public async Task<bool> DeleteReportAsync(Guid id)
    {
        _logger.LogInformation("Deleting report with ID: {Id}", id);
        var command = _mapper.Map<DeleteReportCommand>(id);
        var deletedReport = await _mediator.Send(command);
        _logger.LogInformation("Deleted report with ID: {Id}", id);
        return deletedReport != null;
    }


    public Task<IEnumerable<ReportDTO>> GetAllReportsAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<ReportDTO> GetReportByIdAsync(Guid id)
    {
        _logger.LogInformation("Getting report with ID: {Id}", id);
        var query = new GetReportByIdQuery(id);
        var report = await _mediator.Send(query);
        return _mapper.Map<ReportDTO>(report);
    }

    public Task<UpdateReportDTO> UpdateReportAsync(Guid id, UpdateReportDTO updateReportDto)
    {
        throw new NotImplementedException();
    }

    private void ValidateReportDTO(object reportDto)
    {
        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(reportDto);
        bool isValid = Validator.TryValidateObject(reportDto, validationContext, validationResults, true);

        if (!isValid)
        {
            foreach (var error in validationResults)
            {
                _logger.LogError(error.ErrorMessage);
            }

            throw new ValidationException("ReportDTO is not valid");
        }
    }
}
