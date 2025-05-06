using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PetHelp.Application.Commands.Animals;
using PetHelp.Application.DTOs.Animal;
using PetHelp.Application.Handlers.Animals;
using PetHelp.Application.Interfaces;
using PetHelp.Application.Queries.Animals;

namespace PetHelp.Application.Services;

public class AnimalService : IAnimalService
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;
    private readonly ILogger<AnimalService> _logger;
    public AnimalService(IMapper mapper, IMediator mediator, ILogger<AnimalService> logger)
    {
        _mapper = mapper;
        _mediator = mediator;
        _logger = logger;
    }

    public async Task<CreateAnimalDTO> CreateAnimalAsync(CreateAnimalDTO createAnimalDTO)
    {
        _logger.LogInformation("Creating animal with name: {Name}", createAnimalDTO.Name);
        ValidateAnimalDTO(createAnimalDTO);
        var command = _mapper.Map<CreateAnimalCommand>(createAnimalDTO);
        await _mediator.Send(command);
        _logger.LogInformation("Animal created with name: {Name}", createAnimalDTO.Name);
        return createAnimalDTO;
    }
    public async Task<UpdateAnimalDTO> UpdateAnimalAsync(Guid id, UpdateAnimalDTO updateAnimalDto)
    {
        try
        {
            _logger.LogInformation("Updating animal with ID: {AnimalId}", id);
            ValidateAnimalDTO(updateAnimalDto);
            var command = _mapper.Map<UpdateAnimalCommand>(updateAnimalDto);
            command.Id = id;
            await _mediator.Send(command);
            _logger.LogInformation("Animal updated with ID: {AnimalId}", id);
            var animal = await GetAnimalByIdAsync(id);
            if (animal == null)
            {
                _logger.LogWarning("Animal not found with ID: {AnimalId}", id);
                return null;
            }
            return updateAnimalDto;
        }
        catch
        {
            _logger.LogError("Error updating animal with ID: {AnimalId}", id);
            throw; 
        }
        ;
    }
    public async Task<bool> DeleteAnimalAsync(Guid id)
    {
        try
        {
            _logger.LogInformation("Deleting animal with ID: {AnimalId}", id);

            if (id == Guid.Empty)
            {
                _logger.LogWarning("Attempted to delete animal with empty GUID");
                return false;
            }

            // Mapeando o ID para DeleteAnimalCommand usando AutoMapper
            var command = _mapper.Map<DeleteAnimalCommand>(id);

            // Enviando o comando e aguardando a resposta
            var result = await _mediator.Send(command);

            if (result == null)
            {
                _logger.LogWarning("Animal not found with ID: {AnimalId}", id);
                return false;
            }

            _logger.LogInformation("Animal deleted with ID: {AnimalId}", id);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting animal with ID: {AnimalId}", id);
            throw;
        }
    }
    public async Task<IEnumerable<AnimalDTO>> GetAllAnimalsAsync()
    {
        try
        {
            _logger.LogInformation("Fetching all animals");
            var query = new GetAnimalsQuery();
            var animals = await _mediator.Send(query);
            if (animals == null || !animals.Any())
            {
                _logger.LogWarning("No animals found");
                return new List<AnimalDTO>(); 
            }
            var animalDtos = _mapper.Map<IEnumerable<AnimalDTO>>(animals);
            _logger.LogDebug("Successfully mapped {Count} animals to DTOs", animalDtos.Count());
            return animalDtos; 
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching all animals");
            throw; 
        }
    }
    
    public async Task<AnimalDTO> GetAnimalByIdAsync(Guid id)
    {
        try
        {
            _logger.LogInformation("Fetching animal by ID: {AnimalId}", id);

            if (id == Guid.Empty)
            {
                _logger.LogWarning("Attempted to fetch animal with empty GUID");
                return null;
            }

            var query = new GetAnimalByIdQuery(id);
            var animal = await _mediator.Send(query);

            if (animal == null)
            {
                _logger.LogWarning("Animal not found with ID: {AnimalId}", id);
                return null;
            }

            var animalDto = _mapper.Map<AnimalDTO>(animal);
            _logger.LogDebug("Successfully mapped animal with ID: {AnimalId} to DTO", id);

            return animalDto;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing request for animal ID: {AnimalId}", id);
            throw; // Or return a specific error DTO if you prefer
        }
    }
    
    public Task<IEnumerable<AnimalDTO>> GetAnimalsByAgeRangeAsync(int minAge, int maxAge)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<AnimalDTO>> GetAnimalsByBreedAsync(string breed)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<AnimalDTO>> GetAnimalsByCityAndStateAsync(string city, string state)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<AnimalDTO>> GetAnimalsByCityAsync(string city)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<AnimalDTO>> GetAnimalsBySpeciesAsync(string species)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<AnimalDTO>> GetAnimalsByStateAsync(string state)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<AnimalDTO>> GetAnimalsByUserIdAsync(int userId)
    {
        throw new NotImplementedException();
    }

    

    private void ValidateAnimalDTO(object animalDto)
    {

        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(animalDto, serviceProvider: null, items: null);
        bool isValid = Validator.TryValidateObject(animalDto, validationContext, validationResults, true);
        if (!isValid)
        {
            var errors = validationResults.Select(vr => new ValidationResult(vr.ErrorMessage));
            foreach (var error in errors)
            {
                _logger.LogError(error.ErrorMessage);
            }
            throw new ValidationException("AnimalDTO is not valid");
        }
    }
}
