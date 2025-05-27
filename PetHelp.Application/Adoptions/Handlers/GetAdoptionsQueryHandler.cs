using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PetHelp.Application.Adoptions.Queries;
using PetHelp.Application.Animals.Handlers;
using PetHelp.Application.DTOs.Adoption;
using PetHelp.Domain.Interfaces.Repositories;

namespace PetHelp.Application.Adoptions.Handlers;

public class GetAdoptionsQueryHandler : IRequestHandler<GetAdoptionsQuery, PaginationResponse<AdoptionDTO>>
{
    private readonly IAdoptionRepository _adoptionRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetAnimalsQueryHandler> _logger;

    public GetAdoptionsQueryHandler(
        IAdoptionRepository adoptionRepository,
        IMapper mapper,
        ILogger<GetAnimalsQueryHandler> logger)
    {
        _adoptionRepository = adoptionRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<PaginationResponse<AdoptionDTO>> Handle(
        GetAdoptionsQuery request, CancellationToken cancellationToken)
    {
        if (request.Pagination.PageNumber < 1)
            throw new ArgumentException("Page number must be greater than 0");

        if (request.Pagination.PageSize < 1 || request.Pagination.PageSize > 100)
            throw new ArgumentException("Page size must be between 1 and 100");

        try
        {
            var filter = request.Filter;
            var orderBy = request.OrderBy;

            var (data, totalCount) = await _adoptionRepository.GetPaginatedAsync(
                request.Pagination.PageNumber,
                request.Pagination.PageSize,
                filter,
                orderBy,
                cancellationToken);

            var itemsDto = _mapper.Map<IReadOnlyList<AdoptionDTO>>(data);
            _logger.LogDebug("Retrieved {AnimalCount} animals out of {TotalCount}",
                itemsDto.Count, totalCount);

            return new PaginationResponse<AdoptionDTO>(
                itemsDto,
                totalCount,
                request.Pagination.PageNumber,
                request.Pagination.PageSize);
        }
        catch
        {
            _logger.LogError("Error fetching adoptions with pagination - Page {PageNumber}, Size {PageSize}",
                request.Pagination.PageNumber, request.Pagination.PageSize);
            throw;
        }
    }
}
