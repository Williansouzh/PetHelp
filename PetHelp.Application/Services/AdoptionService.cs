using System.ComponentModel.DataAnnotations;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PetHelp.Application.Adoptions.Commads;
using PetHelp.Application.Adoptions.Queries;
using PetHelp.Application.Animals.Commands;
using PetHelp.Application.DTOs.Adoption;
using PetHelp.Application.Interfaces;
using PetHelp.Application.Pagination;
using static PetHelp.Domain.Enum.AdoptionEnums;
using static PetHelp.Domain.Enum.AnimalEnums;

namespace PetHelp.Application.Services;

public class AdoptionService : IAdoptionService
{
    private readonly ILogger<AdoptionService> _logger;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;
    private readonly IAnimalService _animalService;
    public AdoptionService(ILogger<AdoptionService> logger, IMapper mapper, IMediator mediator, IAnimalService animalService)
    {
        _mapper = mapper;
        _mediator = mediator;
        _logger = logger;
        _animalService = animalService;
    }

    public async Task<AdoptionDTO> CreateAdoptionAsync(AdoptionRequest createAdoptionDto, string userId)
    {
        _logger.LogInformation("Creating adoption request for user: {FullName}", createAdoptionDto.FullName);
        ValidateAdoptionDTO(createAdoptionDto);

        var command = _mapper.Map<CreateAdoptionCommand>(createAdoptionDto);
        command.UserId = userId;
        var adoption = await _mediator.Send(command);
        _logger.LogInformation(userId);
        var adoptionDto = _mapper.Map<AdoptionDTO>(adoption);
        return adoptionDto;
    }

    public async Task<bool> DeleteAdoptionAsync(Guid id)
    {
        _logger.LogInformation("Deleting adoption request with ID: {Id}", id);
        var command = new DeleteAdoptionCommand(id);
        var result = await _mediator.Send(command) != null; 
        _logger.LogInformation("Deleted adoption request with ID: {Id}", id);
        return result;
    }

    public async Task<AdoptionDTO> GetAdoptionByIdAsync(Guid id)
    {
        try
        {
            _logger.LogInformation("Retrieving adoption request with ID: {Id}", id);
            var query = new GetAdoptionByIdQuery(id);
            var adoption = await _mediator.Send(query);

            if (adoption == null)
            {
                _logger.LogWarning("No adoption request found with ID: {Id}", id);
                return null;
            }

            return _mapper.Map<AdoptionDTO>(adoption);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving adoption request with ID: {Id}", id);
            throw;
        }
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

    public async Task<PaginationResponse<AdoptionDTO>> GetAllAdoptionsAsync(int pageNumber, int pageSize)
    {
        _logger.LogInformation("Retrieving all adoptions with pagination: Page {PageNumber}, Size {PageSize}", pageNumber, pageSize);
        var pagination = new PaginationRequest
        {
            PageNumber = pageNumber,
            PageSize = pageSize
        };
        var query = new GetAdoptionsQuery(pagination);
        var reports = await _mediator.Send(query);
        return _mapper.Map<PaginationResponse<AdoptionDTO>>(reports);
    }

    public async Task<AdoptionDTO> UpdateAdoptionAsync(Guid id, AdoptionRequest updateAdoptionDto)
    {
        _logger.LogInformation("Updating adoption request with ID: {Id}", id);
        ValidateAdoptionDTO(updateAdoptionDto);

        var command = _mapper.Map<UpdateAdoptionCommand>(updateAdoptionDto);
        command.Id = id;

        var updatedAdoption = await _mediator.Send(command);
        if (updatedAdoption == null)
        {
            _logger.LogWarning("No adoption request found with ID: {Id} to update", id);
            return null;
        }

        _logger.LogInformation("Updated adoption request with ID: {Id}", id);
        return _mapper.Map<AdoptionDTO>(updatedAdoption);
    }

    public async Task<AdoptionDTO?> UpdateStatusAsync(Guid id, AdoptionStatus status)
    {
        _logger.LogInformation("Updating status for adoption request with ID: {Id} to {Status}", id, status);

        var command = new UpdateStatusCommand
        {
            AdoptionId = id,
            Status = status
        };

        var adoptionDto = await _mediator.Send(command);

        if (adoptionDto != null)
        {
            if (status == AdoptionStatus.Approved)
            {
                await _animalService.UpdateAnimalStatusAsync(adoptionDto.AnimalId, AnimalStatus.Adotado);
            }
            else if (status == AdoptionStatus.Rejected || status == AdoptionStatus.Rejected)
            {
                await _animalService.UpdateAnimalStatusAsync(adoptionDto.AnimalId, AnimalStatus.Disponivel);
            }
        }

        return adoptionDto;
    }


    private void ValidateAdoptionDTO(object adoptionDto)
    {

        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(adoptionDto, serviceProvider: null, items: null);
        bool isValid = Validator.TryValidateObject(adoptionDto, validationContext, validationResults, true);
        if (!isValid)
        {
            var errors = validationResults.Select(vr => new ValidationResult(vr.ErrorMessage));
            foreach (var error in errors)
            {
                _logger.LogError(error.ErrorMessage);
            }
            throw new ValidationException("AdoptionDTO is not valid");
        }
    }
}
