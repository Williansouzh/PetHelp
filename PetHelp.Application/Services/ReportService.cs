using System.ComponentModel.DataAnnotations;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PetHelp.API.DTOs.ReportDTOs;
using PetHelp.Application.DTOs.Report;
using PetHelp.Application.Interfaces;
using PetHelp.Application.Pagination;
using PetHelp.Application.Reports.Commands;
using PetHelp.Application.Reports.Queries;

namespace PetHelp.Application.Services;

public class ReportService : IReportService
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;
    private readonly ILogger<ReportService> _logger;
    private readonly IImageService _imageService;
    public ReportService(IMapper mapper, IMediator mediator, ILogger<ReportService> logger, IImageService imageService)
    {
        _mapper = mapper;
        _mediator = mediator;
        _logger = logger;
        _imageService = imageService;
    }

    public async Task<ReportDTO> CreateReportAsync(CreateReportDTO createReportDto, IFormFile imageFile = null)
    {
        _logger.LogInformation("Creating report with description: {Description}", createReportDto.Description);
        ValidateReportDTO(createReportDto);

        if (imageFile != null)
        {
            var fileName = Guid.NewGuid() + Path.GetExtension(imageFile.FileName);
            using var stream = imageFile.OpenReadStream();
            createReportDto.ImageUrl = await _imageService.UploadImageAsync(stream, fileName, imageFile.ContentType);
        }

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


    public async Task<IEnumerable<ReportResponseDTO>> GetAllReportsAsync(int pageNumber = 1, int pageSize = 10)
    {
        try
        {
            _logger.LogInformation("Fetching all reports");
            var pagination = new PaginationRequest
            {
                PageNumber = pageNumber,
                PageSize = pageSize
            };
            var query = new GetReportsQuery(pagination);
            var reports = await _mediator.Send(query);
            return _mapper.Map<IEnumerable<ReportResponseDTO>>(reports.Data);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching reports");
            throw;
        }
    }

    public async Task<ReportDTO> GetReportByIdAsync(Guid id)
    {
        _logger.LogInformation("Getting report with ID: {Id}", id);
        var query = new GetReportByIdQuery(id);
        var report = await _mediator.Send(query);
        return _mapper.Map<ReportDTO>(report);
    }

    public async Task<UpdateReportDTO> UpdateReportAsync(Guid id, UpdateReportDTO updateReportDto)
    {
        _logger.LogInformation("Updating report with ID: {Id}", id);
        ValidateReportDTO(updateReportDto);
        var command = _mapper.Map<UpdateReportCommand>(updateReportDto);
        command.Id = id;
        var updatedReport = await _mediator.Send(command);
        if (updatedReport == null)
        {
            _logger.LogError("Report with ID: {Id} not found", id);
            throw new KeyNotFoundException($"Report with ID: {id} not found");
        }
        _logger.LogInformation("Report with ID: {Id} updated successfully", id);
        return _mapper.Map<UpdateReportDTO>(updatedReport);
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
