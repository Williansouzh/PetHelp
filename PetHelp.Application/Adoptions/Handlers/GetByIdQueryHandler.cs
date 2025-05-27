using MediatR;
using Microsoft.Extensions.Logging;
using PetHelp.Application.Adoptions.Queries;
using PetHelp.Domain.Entities;
using PetHelp.Domain.Interfaces.Repositories;

namespace PetHelp.Application.Adoptions.Handlers;

public class GetByIdQueryHandler : IRequestHandler<GetAdoptionByIdQuery, Adoption>
{
    private readonly IAdoptionRepository _adoptionRepository;
    private readonly ILogger<GetByIdQueryHandler> _logger;
    public GetByIdQueryHandler(IAdoptionRepository adoptionRepository, ILogger<GetByIdQueryHandler> logger)
    {
        _adoptionRepository = adoptionRepository;
        _logger = logger;
    }

    public async Task<Adoption> Handle(GetAdoptionByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogDebug("Fetching adoption with ID: {AdoptionId}", request.Id);
            if (request.Id == Guid.Empty)
            {
                _logger.LogWarning("Attempted to fetch adoption with empty GUID");
                return null;
            }
            var adoption = await _adoptionRepository.GetByIdAsync(request.Id, cancellationToken);
            if (adoption == null)
            {
                _logger.LogWarning("Adoption not found with ID: {AdoptionId}", request.Id);
            }
            else
            {
                _logger.LogDebug("Successfully retrieved adoption with ID: {AdoptionId}", request.Id);
            }
            return adoption;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching adoption with ID: {AdoptionId}", request.Id);
            throw; 
        }
    }
}
