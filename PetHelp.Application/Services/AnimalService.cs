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
using PetHelp.Application.Interfaces;

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

    public Task<bool> DeleteAnimalAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<AnimalDTO>> GetAllAnimalsAsync()
    {
        throw new NotImplementedException();
    }

    public Task<AnimalDTO> GetAnimalByIdAsync(Guid id)
    {
        throw new NotImplementedException();
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

    public Task<AnimalDTO> UpdateAnimalAsync(AnimalDTO animalDto)
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
