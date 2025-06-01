using PetHelp.Application.DTOs.Animal;
using static PetHelp.Domain.Enum.AnimalEnums;

namespace PetHelp.Application.Interfaces;

public interface IAnimalService
{
    Task<PaginationResponse<AnimalDTO>> GetAllAnimalsAsync(int pageNumber, int pageSize);
    Task<AnimalDTO> GetAnimalByIdAsync(Guid id);
    Task<AnimalRequest> CreateAnimalAsync(AnimalRequest CreateAnimalDto);
    Task<AnimalRequest> UpdateAnimalAsync(Guid id, AnimalRequest animalDto);
    Task UpdateAnimalStatusAsync(Guid animalId, AnimalStatus newStatus);
    Task<bool> DeleteAnimalAsync(Guid id);
    Task<IEnumerable<AnimalDTO>> GetAnimalsByUserIdAsync(int userId);
    Task<IEnumerable<AnimalDTO>> GetAnimalsByCityAndStateAsync(string city, string state);
    Task<IEnumerable<AnimalDTO>> GetAnimalsBySpeciesAsync(string species);
    Task<IEnumerable<AnimalDTO>> GetAnimalsByBreedAsync(string breed);
    Task<IEnumerable<AnimalDTO>> GetAnimalsByAgeRangeAsync(int minAge, int maxAge);
    Task<IEnumerable<AnimalDTO>> GetAnimalsByCityAsync(string city);
    Task<IEnumerable<AnimalDTO>> GetAnimalsByStateAsync(string state);

}
