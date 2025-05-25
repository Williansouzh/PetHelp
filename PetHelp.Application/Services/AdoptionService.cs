using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PetHelp.Application.DTOs.Adoption;
using PetHelp.Application.Interfaces;

namespace PetHelp.Application.Services;

public class AdoptionService : IAdoptionService
{
    private readonly ILogger<AdoptionService> _logger;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;
    public AdoptionService(ILogger<AdoptionService> logger, IMapper mapper, IMediator mediator)
    {
        _mapper = mapper;
        _mediator = mediator;
        _logger = logger;
    }

    public Task<AdoptionDTO> CreateAdoptionAsync(AdoptionRequest createAdoptionDto)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteAdoptionAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<AdoptionDTO> GetAdoptionByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<AdoptionDTO>> GetAdoptionsByAgeRangeAsync(int minAge, int maxAge)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<AdoptionDTO>> GetAdoptionsByBreedAsync(string breed)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<AdoptionDTO>> GetAdoptionsByCityAndStateAsync(string city, string state)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<AdoptionDTO>> GetAdoptionsByCityAsync(string city)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<AdoptionDTO>> GetAdoptionsBySpeciesAsync(string species)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<AdoptionDTO>> GetAdoptionsByStateAsync(string state)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<AdoptionDTO>> GetAdoptionsByUserIdAsync(int userId)
    {
        throw new NotImplementedException();
    }

    public Task<PaginationResponse<AdoptionDTO>> GetAllAdoptionsAsync(int pageNumber, int pageSize)
    {
        throw new NotImplementedException();
    }

    public Task<AdoptionDTO> UpdateAdoptionAsync(Guid id, AdoptionRequest updateAdoptionDto)
    {
        throw new NotImplementedException();
    }
}
