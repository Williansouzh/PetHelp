using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PetHelp.Application.Animals.Queries;
using PetHelp.Application.DTOs.Animal;
using PetHelp.Domain.Interfaces.Repositories;
namespace PetHelp.Application.Animals.Handlers;
public class GetAnimalsQueryHandler : IRequestHandler<GetAnimalsQuery, PaginationResponse<AnimalDTO>>
{
    private readonly IAnimalRepository _animalRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetAnimalsQueryHandler> _logger;

    public GetAnimalsQueryHandler(
        IAnimalRepository animalRepository,
        IMapper mapper,
        ILogger<GetAnimalsQueryHandler> logger)
    {
        _animalRepository = animalRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<PaginationResponse<AnimalDTO>> Handle(
        GetAnimalsQuery request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Fetching animals with pagination - Page {PageNumber}, Size {PageSize}",
            request.Pagination.PageNumber, request.Pagination.PageSize);

        // Validação
        if (request.Pagination.PageNumber < 1)
            throw new ArgumentException("Page number must be greater than 0");

        if (request.Pagination.PageSize < 1 || request.Pagination.PageSize > 100)
            throw new ArgumentException("Page size must be between 1 and 100");

        try
        {
            // Obter filtro e ordenação da query (se existirem)
            var filter = request.Filter;
            var orderBy = request.OrderBy;

            var (data, totalCount) = await _animalRepository.GetPaginatedAsync(
                request.Pagination.PageNumber,
                request.Pagination.PageSize,
                filter,
                orderBy,
                cancellationToken);

            var itemsDto = _mapper.Map<IReadOnlyList<AnimalDTO>>(data);

            _logger.LogDebug("Retrieved {AnimalCount} animals out of {TotalCount}",
                itemsDto.Count, totalCount);

            return new PaginationResponse<AnimalDTO>(
                itemsDto,
                totalCount,
                request.Pagination.PageNumber,
                request.Pagination.PageSize);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching paginated animals");
            throw;
        }
    }
}