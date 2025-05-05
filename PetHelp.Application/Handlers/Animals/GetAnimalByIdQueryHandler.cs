using MediatR;
using Microsoft.Extensions.Logging;
using PetHelp.Application.Queries.Animals;
using PetHelp.Domain.Entities;
using PetHelp.Domain.Interfaces.Repositories;

public class GetAnimalByIdQueryHandler : IRequestHandler<GetAnimalByIdQuery, Animal>
{
    private readonly IAnimalRepository _animalRepository;
    private readonly ILogger<GetAnimalByIdQueryHandler> _logger;

    public GetAnimalByIdQueryHandler(
        IAnimalRepository animalRepository,
        ILogger<GetAnimalByIdQueryHandler> logger)
    {
        _animalRepository = animalRepository;
        _logger = logger;
    }

    public async Task<Animal> Handle(GetAnimalByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogDebug("Fetching animal with ID: {AnimalId}", request.Id);

            if (request.Id == Guid.Empty)
            {
                _logger.LogWarning("Attempted to fetch animal with empty GUID");
                return null;
            }

            var animal = await _animalRepository.GetByIdAsync(request.Id, cancellationToken);

            if (animal == null)
            {
                _logger.LogWarning("Animal not found with ID: {AnimalId}", request.Id);
            }
            else
            {
                _logger.LogDebug("Successfully retrieved animal with ID: {AnimalId}", request.Id);
            }

            return animal;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching animal with ID: {AnimalId}", request.Id);
            throw; // Re-throw to preserve stack trace
        }
    }
}