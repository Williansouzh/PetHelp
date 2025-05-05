using MediatR;
using Microsoft.Extensions.Logging;
using PetHelp.Application.Queries.Animals;
using PetHelp.Domain.Entities;
using PetHelp.Domain.Interfaces.Repositories;

namespace PetHelp.Application.Handlers.Animals;

public class GetAnimalsQueryHandler : IRequestHandler<GetAnimalsQuery, IEnumerable<Animal>>
{
    private readonly IAnimalRepository _animalRepository;
    private readonly ILogger<GetAnimalsQueryHandler> _logger; // Fixed logger type

    public GetAnimalsQueryHandler(
        IAnimalRepository animalRepository, // Fixed parameter name
        ILogger<GetAnimalsQueryHandler> logger)
    {
        _animalRepository = animalRepository;
        _logger = logger;
    }

    public async Task<IEnumerable<Animal>> Handle(
        GetAnimalsQuery request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Fetching all animals");

        try
        {
            var animals = await _animalRepository.GetAllAsync(cancellationToken);
            _logger.LogDebug("Retrieved {AnimalCount} animals", animals.Count());
            return animals;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching animals");
            throw; // Re-throw to preserve stack trace
        }
    }
}