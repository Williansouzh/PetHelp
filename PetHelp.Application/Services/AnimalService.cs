using System.ComponentModel.DataAnnotations;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PetHelp.Application.Animals.Commands;
using PetHelp.Application.Animals.Queries;
using PetHelp.Application.DTOs.Animal;
using PetHelp.Application.Interfaces;
using PetHelp.Application.Pagination;
using PetHelp.Application.Queries.Animals;
using PetHelp.Domain.Enum;

namespace PetHelp.Application.Services;

public class AnimalService : IAnimalService
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;
    private readonly ILogger<AnimalService> _logger;
    private readonly IImageService _imageService;
    public AnimalService(IMapper mapper, IMediator mediator, ILogger<AnimalService> logger, IImageService imageService)
    {
        _mapper = mapper;
        _mediator = mediator;
        _logger = logger;
        _imageService = imageService;
    }

    public async Task<AnimalRequest> CreateAnimalAsync(AnimalRequest createAnimalDTO)
    {
        _logger.LogInformation("Creating animal with name: {Name}", createAnimalDTO.Name);
        ValidateAnimalDTO(createAnimalDTO);

        if (createAnimalDTO.Image != null && createAnimalDTO.Image.Length > 0)
        {
            using var stream = createAnimalDTO.Image.OpenReadStream();
            var imageUrl = await _imageService.UploadImageAsync(stream, createAnimalDTO.Image.FileName, createAnimalDTO.Image.ContentType);
            createAnimalDTO.ImageUrl = imageUrl; 
        }
        else
        {
            createAnimalDTO.ImageUrl = "https://storage.googleapis.com/pethelp-images/default-animal.jpg";
        }

        var command = _mapper.Map<CreateAnimalCommand>(createAnimalDTO);
        await _mediator.Send(command);

        _logger.LogInformation("Animal created with name: {Name}", createAnimalDTO.Name);
        return createAnimalDTO;
    }
    public async Task<AnimalRequest> UpdateAnimalAsync(Guid id, AnimalRequest updateAnimalDto)
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
    public async Task<PaginationResponse<AnimalDTO>> GetAllAnimalsAsync(int pageNumber = 1, int pageSize = 10)
    {
        try
        {
            _logger.LogInformation("Fetching animals: Page {PageNumber}, Size {PageSize}", pageNumber, pageSize);

            var pagination = new PaginationRequest
            {
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            var query = new GetAnimalsQuery(pagination);
            return await _mediator.Send(query);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching animals");
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

    public Task UpdateAnimalStatusAsync(Guid animalId, AnimalEnums.AnimalStatus newStatus)
    {
        _logger.LogInformation("Updating animal status for Animal ID: {AnimalId} to {NewStatus}", animalId, newStatus);
        var command = new UpdateAnimalStatusCommand
        {
            AnimalId = animalId,
            Status = newStatus
        };
        return _mediator.Send(command);
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
