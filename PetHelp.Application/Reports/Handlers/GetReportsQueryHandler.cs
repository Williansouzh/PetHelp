using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PetHelp.Application.DTOs.Report;
using PetHelp.Application.Reports.Queries;
using PetHelp.Domain.Interfaces.Repositories;

namespace PetHelp.Application.Reports.Handlers;

public class GetReportsQueryHandler : IRequestHandler<GetReportsQuery, PaginationResponse<ReportDTO>>
{
    private readonly IReportRepository _reportRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetReportsQueryHandler> _logger;
    public GetReportsQueryHandler(
        IReportRepository reportRepository,
        IMapper mapper,
        ILogger<GetReportsQueryHandler> logger)
    {
        _reportRepository = reportRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<PaginationResponse<ReportDTO>> Handle(
        GetReportsQuery request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Fetching reports with pagination - Page {PageNumber}, Size {PageSize}",
            request.Pagination.PageNumber, request.Pagination.PageSize);
        // Validation
        if (request.Pagination.PageNumber < 1)
            throw new ArgumentException("Page number must be greater than 0");
        if (request.Pagination.PageSize < 1 || request.Pagination.PageSize > 100)
            throw new ArgumentException("Page size must be between 1 and 100");
        try
        {
            var filter = request.Filter;
            var orderBy = request.OrderBy;
            var (data, totalCount) = await _reportRepository.GetPaginatedAsync(
                request.Pagination.PageNumber,
                request.Pagination.PageSize,
                filter,
                orderBy,
                cancellationToken);
            var itemsDto = _mapper.Map<IReadOnlyList<ReportDTO>>(data);
            _logger.LogDebug("Retrieved {ReportCount} reports out of {TotalCount}",
                itemsDto.Count, totalCount);
            return new PaginationResponse<ReportDTO>(
                itemsDto,
                totalCount,
                request.Pagination.PageNumber,
                request.Pagination.PageSize);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching reports");
            throw;
        }
    }
}
